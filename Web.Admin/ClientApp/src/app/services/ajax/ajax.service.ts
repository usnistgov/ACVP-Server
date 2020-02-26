import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { DependencyList } from '../../models/dependency/dependency-list';
import { Dependency } from '../../models/dependency/dependency';
import { Attribute } from '../../models/dependency/attribute';
import { OperatingEnvironment } from '../../models/operatingEnvironment/operatingEnvironment';
import { Result } from '../../models/responses/Result';
import { Product } from '../../models/Product/Product';
import { Address } from '../../models/Address/Address';
import { ProductList } from '../../models/Product/ProductList';
import { OperatingEnvironmentList } from '../../models/OperatingEnvironment/OperatingEnvironmentList';
import { PersonList } from '../../models/Person/PersonList';
import { Person } from '../../models/Person/Person';
import { Organization } from '../../models/Organization/Organization';
import { AddressCreateParameters } from '../../models/Address/AddressCreateParameters';

const httpOptions = {
  headers: new HttpHeaders()
};

@Injectable({
  providedIn: 'root'
})
export class AjaxService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  getSession(sessionId:number) {
    return this.http.get('/assets/fillerJson/testSession556', {});
  }

  getVectorSets(sessionId:number) {
    return this.http.get('/assets/fillerJson/testSession556VectorSets', {});
  }

  // Dependency-related AJAX calls
  getDependencies(pageSize: number, pageNumber: number) {
    return this.http.get<DependencyList>(this.apiRoot + '/dependencies?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  };

  getDependency(id: number) {
    return this.http.get<Dependency>(this.apiRoot + '/dependencies/' + id);
  };

  addAttribute(dependencyId: number, attribute: Attribute) {
    return this.http.post(this.apiRoot + '/dependencies/' + dependencyId + '/attributes', attribute);
  }

  deleteAttribute(dependencyId: number, attributeId: number) {
    return this.http.delete(this.apiRoot + '/dependencies/' + dependencyId + '/attributes/' + attributeId);
  }

  updateDependency(dependency: Dependency) {
    return this.http.patch(this.apiRoot + '/dependencies/' + dependency.id, dependency);
  }
  // END - Dependency-related AJAX calls

  // Operating Environment (OE) -related calls
  getOE(id: number) {
    return this.http.get<OperatingEnvironment>(this.apiRoot + '/OperatingEnvironments/' + id);
  };

  getOEs(pageSize: number, pageNumber: number) {
    return this.http.get<OperatingEnvironmentList>(this.apiRoot + '/OperatingEnvironments?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  }

  addDependencyToOE(dependencyId: number, OEID: number) {

    // Assemble message body
    var requestBody = { dependencyId : 0 };
    requestBody.dependencyId = dependencyId;

    // POST it
    return this.http.post(this.apiRoot + '/OperatingEnvironments/' + OEID + '/Dependencies', requestBody);
  }

  removeDependencyFromOE(dependencyId: number, OEID: number) {
    return this.http.delete(this.apiRoot + '/OperatingEnvironments/' + OEID + '/Dependencies/' + dependencyId);
  }

  updateOE(oe: OperatingEnvironment) {
    return this.http.patch(this.apiRoot + '/OperatingEnvironments/' + oe.id, oe);
  }

  createDependency(dependency: Dependency) {
    return this.http.post<Result>(this.apiRoot + '/Dependencies', dependency);
  }
  // END Operating Environment (OE) -related calls

  // Product-Related calls
  getProduct(id: number) {
    return this.http.get<Product>(this.apiRoot + '/Products/' + id);
  };

  getProducts(pageSize: number, pageNumber: number) {
    return this.http.get<ProductList>(this.apiRoot + '/Products?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  }

  updateProduct(product: Product) {
    return this.http.patch(this.apiRoot + '/Products/' + product.id, product);
  }
  updateAddress(address: Address) {
    return this.http.patch(this.apiRoot + '/Addresses/' + address.id, address);
  }
  // END Product-related calls

  // Person-Related calls
  getPerson(id: number) {
    return this.http.get<Person>(this.apiRoot + '/Persons/' + id);
  }

  updatePerson(person: Person) {
    return this.http.patch(this.apiRoot + '/Persons/' + person.id, person);
  }

  getPersons(pageSize: number, pageNumber: number) {
    return this.http.get<PersonList>(this.apiRoot + '/Persons?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  }
  // END Person-related calls

  // Organization-related calls
  getOrganization(id: number) {
    return this.http.get<Organization>(this.apiRoot + '/Organizations/' + id);
  }

  addNewAddress(parameters: AddressCreateParameters) {
    return this.http.post(this.apiRoot + '/Addresses/', parameters);
  }

  deleteAddressFromOrganization(index: number, organizationID: number) {
    return this.http.delete(this.apiRoot + '/Organizations/' + organizationID + '/Addresses/' + index);
  }

  updateOrganization(organization: Organization) {
    return this.http.patch(this.apiRoot + '/Organizations/' + organization.id, organization);
  }
}
