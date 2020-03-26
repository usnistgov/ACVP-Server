import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Product } from '../../models/Product/Product';
import { Address } from '../../models/Address/Address';
import { ProductList } from '../../models/Product/ProductList';
import { PersonList } from '../../models/Person/PersonList';
import { Person } from '../../models/Person/Person';
import { Organization } from '../../models/Organization/Organization';
import { AddressCreateParameters } from '../../models/Address/AddressCreateParameters';
import { TestSessionList } from '../../models/TestSession/TestSessionList';
import { TestSession } from '../../models/TestSession/TestSession';
import { VectorSet } from '../../models/TestSession/VectorSet';
import { WorkflowItemBase } from '../../models/Workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../models/Workflow/IWorkflowItemPayload';
import { WorkflowItemList } from '../../models/Workflow/WorkflowItemList';
import { OrganizationList } from '../../models/Organization/OrganizationList';

const httpOptions = {
  headers: new HttpHeaders()
};

@Injectable({
  providedIn: 'root'
})
export class AjaxService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Product-Related calls
  getProduct(id: number) {
    return this.http.get<Product>(this.apiRoot + '/Implementations/' + id);
  };

  getProducts(pageSize: number, pageNumber: number) {
    var params = { "pageSize": pageSize, "page": pageNumber };

    return this.http.post<ProductList>(this.apiRoot + '/Implementations', params);
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
    var params = { "pageSize": pageSize, "page": pageNumber };

    return this.http.post<PersonList>(this.apiRoot + '/Persons', params);
  }
  // END Person-related calls

  // Organization-related calls
  getOrganization(id: number) {
    return this.http.get<Organization>(this.apiRoot + '/Organizations/' + id);
  }

  getOrganizations(pageSize: number, pageNumber: number) {
    var params = { "pageSize": pageSize, "page": pageNumber };

    return this.http.post<OrganizationList>(this.apiRoot + '/Organizations', params);
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
  // END Organization-related calls

  // Begin TestSession-related calls
  getTestSessions(pageSize: number, pageNumber: number) {
    // Build the request body
    var params = { "pageSize": pageSize, "page": pageNumber };

    return this.http.post<TestSessionList>(this.apiRoot + '/TestSessions', params);
  }

  getTestSession(sessionId: number) {
    return this.http.get<TestSession>(this.apiRoot + '/TestSessions/' + sessionId);
  }

  getVectorSet(vectorSetId: number) {
    return this.http.get<VectorSet>(this.apiRoot + '/TestSessions/VectorSet/' + vectorSetId);
  }

  // END TestSession-related calls
}
