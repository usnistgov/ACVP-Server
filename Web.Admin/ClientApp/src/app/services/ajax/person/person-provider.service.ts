import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '../../../models/person/Person';
import { PersonList } from '../../../models/person/PersonList';
import { PersonListParameters } from '../../../models/person/PersonListParameters';
import { Address } from '../../../models/address/Address';

@Injectable({
  providedIn: 'root'
})
export class PersonProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Person-Related calls
  getPerson(id: number) {
    return this.http.get<Person>(this.apiRoot + '/Persons/' + id);
  }

  updatePerson(person: Person) {
    return this.http.patch(this.apiRoot + '/Persons/' + person.id, person);
  }

  getAddress(addressId: number) {
    return this.http.get<Address>(this.apiRoot + '/Addresses/' + addressId);
  }

  getPersons(params: PersonListParameters) {
    console.log(params);
    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.name === "") { params.name = null; }
    if (params.id === "") { params.id = null; }
    if (params.organizationName === "") { params.organizationName = null; }

    var slightlyReformatted = {
      "page": params.page,
      "pageSize": params.pageSize,
      "name": params.name,
      "organizationName": params.organizationName,
      "id": parseInt(params.id) // Because text inputs must bind to strings, but the API takes integers
    }

    return this.http.post<PersonList>(this.apiRoot + '/Persons', slightlyReformatted);
  };
  // END Person-related calls

}
