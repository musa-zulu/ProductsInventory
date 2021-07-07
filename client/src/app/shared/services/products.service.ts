import { Injectable } from "@angular/core";
import { HttpClient, HttpEventType } from "@angular/common/http";
import { ServerConfig } from "./server-config";
import { Observable } from "rxjs";
import { retry, catchError } from "rxjs/operators";
import { Product } from "../models/product";

@Injectable({
  providedIn: "root",
})
export class ProductsService {
  private readonly _apiURL = "products";
  constructor(private _http: HttpClient, private _serverConfig: ServerConfig) {}

  public getProducts(): Observable<any> {
    return this._http
      .get<Product[]>(
        this._serverConfig.getBaseUrl() + this._apiURL,
        this._serverConfig.getRequestOptions()
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  getProduct(product: Product): Observable<any> {
    return this._http
      .post<Product>(
        this._serverConfig.getBaseUrl() + this._apiURL + "/",
        product.productId,
        this._serverConfig.getRequestOptions()
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  deleteProduct(product: Product) {
    return this._http
      .delete<Product>(
        this._serverConfig.getBaseUrl() +
          this._apiURL +
          "/" +
          product.productId,
        this._serverConfig.getRequestOptions()
      )
      .toPromise();
  }

  updateProduct(product: Product) {
    return this._http
      .put(
        this._serverConfig.getBaseUrl() + this._apiURL + "/",
        product,
        this._serverConfig.getRequestOptions()
      )
      .toPromise();
  }

  addProduct(product: Product): Promise<any> {
    return this._http
      .post(
        this._serverConfig.getBaseUrl() + this._apiURL,
        product,
        this._serverConfig.getRequestOptions()
      )
      .toPromise();
  }

  downloadExcel(){
    return this._http
    .post(
      this._serverConfig.getBaseUrl() + this._apiURL + '/downloadExcel',      
      { responseType: 'blob'} 
    );
  }

  private handleError(error: any) {
    return Observable.throw(error);
  }
}
