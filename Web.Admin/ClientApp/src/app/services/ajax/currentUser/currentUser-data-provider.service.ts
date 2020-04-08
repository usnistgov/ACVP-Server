import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
  })
  export class CurrentUserDataProviderService {
    apiRoot = "/api";

    constructor(private http: HttpClient) { }

    getCurrentUserEmail() {
        return this.http.get(this.apiRoot + '/currentUser/email', { responseType: 'text' });
    }

    getCurrentUserClaims() {
        return this.http.get<Map<string, string>>(this.apiRoot + '/currentUser');
    }
  }