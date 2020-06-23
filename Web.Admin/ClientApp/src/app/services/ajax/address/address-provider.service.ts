import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Address } from '../../../models/address/Address';

@Injectable({
  providedIn: 'root'
})
export class AddressProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  getAddress(id: number) {
    return this.http.get<Address>(this.apiRoot + '/addresses/' + id);
  };
}
