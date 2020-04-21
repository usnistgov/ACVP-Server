import { Injectable } from '@angular/core';
import { OperatingEnvironment } from '../../../models/operatingEnvironment/OperatingEnvironment';
import { HttpClient } from '@angular/common/http';
import { OperatingEnvironmentList } from '../../../models/operatingEnvironment/OperatingEnvironmentList';
import { DependencyList } from '../../../models/dependency/Dependency-list';
import { Dependency } from '../../../models/dependency/Dependency';
import { Result } from '../../../models/responses/Result';
import { OperatingEnvironmentListParameters } from '../../../models/operatingEnvironment/OperatingEnvironmentListParameters';

@Injectable({
  providedIn: 'root'
})
export class OperatingEnvironmentProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Operating Environment (OE) -related calls
  getOE(id: number) {
    return this.http.get<OperatingEnvironment>(this.apiRoot + '/OperatingEnvironments/' + id);
  };

  // Dependency-related AJAX calls
  getOEs(params: OperatingEnvironmentListParameters) {
    console.log(params);
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

    return this.http.post<OperatingEnvironmentList>(this.apiRoot + '/OperatingEnvironments', slightlyReformatted);
  };

  addDependencyToOE(dependencyId: number, OEID: number) {

    // Assemble message body
    var requestBody = { dependencyId: 0 };
    requestBody.dependencyId = dependencyId;

    // POST it
    return this.http.post(this.apiRoot + '/OperatingEnvironments/' + OEID + '/Dependencies', requestBody);
  }

  getDependencies(pageSize: number, pageNumber: number) {
    var params = { "pageSize": pageSize, "page": pageNumber };

    return this.http.post<DependencyList>(this.apiRoot + '/Dependencies', params);
  }

  getDependency(id: number) {
    return this.http.get<Dependency>(this.apiRoot + '/Dependency/' + id);
  };

  removeDependencyFromOE(dependencyId: number, OEID: number) {
    return this.http.delete(this.apiRoot + '/OperatingEnvironments/' + OEID + '/Dependencies/' + dependencyId);
  }

  updateOE(oe: OperatingEnvironment) {
    return this.http.patch(this.apiRoot + '/OperatingEnvironments/' + oe.id, oe);
  }

  createDependency(dependency: Dependency) {
    return this.http.post<Result>(this.apiRoot + '/Dependencies/create', dependency);
  }
  // END Operating Environment (OE) -related calls
}
