import { Injectable } from '@angular/core';
import { ServerConfig } from './server-config';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { Category } from '../models/category';
import { GetCategoryDto } from 'src/app/shared/Dtos/get-category-dto';


@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly _apiURL = 'categories';

  constructor(private _http: HttpClient,
              private _serverConfig: ServerConfig) { }

  public getCategories(): Observable<any>  {
    return this._http
               .get<Category[]>(this._serverConfig.getBaseUrl() +  this._apiURL, this._serverConfig.getRequestOptions())
               .pipe(retry(1), catchError(this.handleError));
  }
  
  public getCategoryById(getCategoryDto: GetCategoryDto): Promise<any> {
    return this._http
        .post<Category>(this._serverConfig.getBaseUrl() + this._apiURL + '/', getCategoryDto.categoryId,
        this._serverConfig.getRequestOptions())
        .toPromise();
  }

  public addCategory(category: Category): Promise<any> {
    return this._http
        .post(this._serverConfig.getBaseUrl() + this._apiURL, category, this._serverConfig.getRequestOptions())
        .toPromise();
  }

  public updateCategory(category: Category): Promise<any> {
    return this._http
        .put(this._serverConfig.getBaseUrl() + this._apiURL + '/', category, this._serverConfig.getRequestOptions())
        .toPromise();
  }

  public deleteFoodCategory(categoryId: string) {
    return this._http
    .delete<Category>(this._serverConfig.getBaseUrl() + this._apiURL + '/' + categoryId, this._serverConfig.getRequestOptions())
    .toPromise();
  }
              
  private handleError(error: any) {    
    return Observable.throw(error);
  }
}
