import { Injectable } from '@angular/core';
import { OperatingEnvironment } from '../../../models/operatingEnvironment/operatingEnvironment';
import { HttpClient } from '@angular/common/http';
import { OperatingEnvironmentList } from '../../../models/OperatingEnvironment/OperatingEnvironmentList';
import { DependencyList } from '../../../models/dependency/dependency-list';
import { Dependency } from '../../../models/dependency/dependency';
import { Result } from '../../../models/responses/Result';

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

  getOEs(pageSize: number, pageNumber: number) {
    var params = { "pageSize": pageSize, "page": pageNumber };

    return this.http.post<OperatingEnvironmentList>(this.apiRoot + '/OperatingEnvironments', params);
  }

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
    return this.http.post<Result>(this.apiRoot + '/Dependencies', dependency);
  }
  // END Operating Environment (OE) -related calls
}
