import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
}
