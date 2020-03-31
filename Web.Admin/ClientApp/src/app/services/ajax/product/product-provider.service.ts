import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Product } from '../../../models/Product/Product';
import { ProductList } from '../../../models/Product/ProductList';
import { Address } from '../../../models/Address/Address';
import { ProductListParameters } from '../../../models/Product/ProductListParameters';

@Injectable({
  providedIn: 'root'
})
export class ProductProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Product-Related calls
  getProduct(id: number) {
    return this.http.get<Product>(this.apiRoot + '/Implementations/' + id);
  };

  getProducts(params: ProductListParameters) {
    console.log(params);
    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.name === "") { params.name = null; }
    if (params.id === "") { params.id = null; }
    if (params.description === "") { params.description = null; }

    var slightlyReformatted = {
      "page": params.page,
      "pageSize": params.pageSize,
      "name": params.name,
      "description": params.description,
      "id": parseInt(params.id) // Because text inputs must bind to strings, but the API takes integers
    }

    return this.http.post<ProductList>(this.apiRoot + '/Implementations', slightlyReformatted);
  };

  updateProduct(product: Product) {
    return this.http.patch(this.apiRoot + '/Implementations/' + product.id, product);
  }
  updateAddress(address: Address) {
    return this.http.patch(this.apiRoot + '/Addresses/' + address.id, address);
  }
  // END Product-related calls
}
