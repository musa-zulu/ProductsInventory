import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ServerConfig {

  public getRequestOptions(): any {
    const headers = new HttpHeaders(
        {
            'Content-Type': 'application/json',
        });
    const options = (
        {
            headers
        });

    return options;
}

  public getBaseUrl(): string {
    return environment.baseUrl;
  }
}
