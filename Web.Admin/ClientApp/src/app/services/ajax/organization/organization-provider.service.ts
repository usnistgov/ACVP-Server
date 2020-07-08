import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Organization } from '../../../models/organization/Organization';
import { OrganizationList } from '../../../models/organization/OrganizationList';
import { AddressCreateParameters } from '../../../models/address/AddressCreateParameters';
import { OrganizationListParameters } from '../../../models/organization/OrganizationListParameters';
import { Result } from '../../../models/responses/Result';
import { OrganizationCreateParameters } from '../../../models/organization/OrganizationCreateParameters';

@Injectable({
  providedIn: 'root'
})
export class OrganizationProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Organization-related calls
  getOrganization(id: number) {
    return this.http.get<Organization>(this.apiRoot + '/Organizations/' + id);
  }

  // Dependency-related AJAX calls
  getOrganizations(params: OrganizationListParameters) {
    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.name === "") { params.name = null; }
    if (params.id === "") { params.id = null; }

    var slightlyReformatted = {
      "page": params.page,
      "pageSize": params.pageSize,
      "name": params.name,
      "id": parseInt(params.id) // Because text inputs must bind to strings, but the API takes integers
    }

    return this.http.post<OrganizationList>(this.apiRoot + '/organizations', slightlyReformatted);
  };

  addNewAddress(parameters: AddressCreateParameters) {
    return this.http.post(this.apiRoot + '/Addresses/', parameters);
  }

  createNewOrganization(parameters: OrganizationCreateParameters) {
    return this.http.put<Result>(this.apiRoot + '/organizations', parameters);
  }

  deleteAddressFromOrganization(index: number, organizationID: number) {
    return this.http.delete(this.apiRoot + '/Organizations/' + organizationID + '/Addresses/' + index);
  }

  updateOrganization(organization: Organization) {
    return this.http.patch(this.apiRoot + '/Organizations/' + organization.id, organization);
  }
  // END Organization-related calls

}
