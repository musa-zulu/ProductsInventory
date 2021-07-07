import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';
import { retry } from 'rxjs/operators';
import { GetUserDto } from '../Dtos/get-user-dto';
import { Login } from '../models/login';
import { Register } from '../models/register';
import { ServerConfig } from './server-config';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly _apiURL = 'account';
  constructor(private _http: HttpClient,
              private _serverConfig: ServerConfig) { }

  registerUser(register: Register): Promise<any> {
    return this._http
               .post(this._serverConfig.getBaseUrl() + this._apiURL + '/register', register, this._serverConfig.getRequestOptions())
               .toPromise();
  }

  login(login: Login): Promise<any> {
    return this._http
    .post(this._serverConfig.getBaseUrl() + this._apiURL + '/login', login, this._serverConfig.getRequestOptions())
    .toPromise();
  }  

  logout(): Promise<any> {
    return this._http
    .post(this._serverConfig.getBaseUrl() + this._apiURL + '/logout', this._serverConfig.getRequestOptions())
    .toPromise();
  }  

  getLoggedInUser(getUserDto: GetUserDto) : Promise<any>{
    return this._http
    .post(this._serverConfig.getBaseUrl() + this._apiURL+ '/getUser', getUserDto, this._serverConfig.getRequestOptions())
    .toPromise();
  }

  private handleError(error: any) {    
    return Observable.throw(error);
  }
}
