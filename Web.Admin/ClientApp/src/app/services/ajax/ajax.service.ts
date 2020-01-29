import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { DependencyList } from '../../models/dependency-list';

const httpOptions = {
  headers: new HttpHeaders()
};

@Injectable({
  providedIn: 'root'
})
export class AjaxService {

  constructor(private http: HttpClient) { }

  getSession(sessionId:number) {
    return this.http.get('/assets/fillerJson/testSession556', {});
  }

  getVectorSets(sessionId:number) {
    return this.http.get('/assets/fillerJson/testSession556VectorSets', {});
  }

  getDependencies(pageSize: number, pageNumber: number) {
    return this.http.get<DependencyList>('/api/dependencies?pageNumber=' + pageNumber + "&pageSize=" + pageSize);
  };

}
