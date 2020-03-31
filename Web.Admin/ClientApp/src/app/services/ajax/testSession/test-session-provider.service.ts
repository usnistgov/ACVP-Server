import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TestSessionList } from '../../../models/TestSession/TestSessionList';
import { TestSession } from '../../../models/TestSession/TestSession';
import { VectorSet } from '../../../models/TestSession/VectorSet';

@Injectable({
  providedIn: 'root'
})
export class TestSessionProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

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
