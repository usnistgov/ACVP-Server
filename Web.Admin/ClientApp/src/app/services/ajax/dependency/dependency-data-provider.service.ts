import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Dependency } from '../../../models/dependency/dependency';
import { DependencyList } from '../../../models/dependency/dependency-list';
import { DependencyListParameters } from '../../../models/dependency/DependencyListParameters';
import { Attribute } from '../../../models/dependency/attribute';

@Injectable({
  providedIn: 'root'
})
export class DependencyDataProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Dependency-related AJAX calls
  getDependencies(params: DependencyListParameters) {
    console.log(params);
    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.name === "") { params.name = null; }
    if (params.type === "") { params.type = null; }
    if (params.description === "") { params.description = null; }

    return this.http.post<DependencyList>(this.apiRoot + '/dependencies', params);
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

}
