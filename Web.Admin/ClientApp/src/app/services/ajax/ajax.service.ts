import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { DependencyList } from '../../models/dependency/dependency-list';
import { Dependency } from '../../models/dependency/dependency';
import { Attribute } from '../../models/dependency/attribute';

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

}
