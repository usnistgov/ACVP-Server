import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TestSessionList } from '../../../models/TestSession/TestSessionList';
import { TestSession } from '../../../models/TestSession/TestSession';
import { VectorSet } from '../../../models/TestSession/VectorSet';
import { TestSessionListParameters } from '../../../models/TestSession/TestSessionListParameters';
import { TestSessionStatus } from '../../../models/TestSession/TestSessionStatus';

@Injectable({
  providedIn: 'root'
})
export class TestSessionProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // Begin TestSession-related calls
  getTestSessions(params: TestSessionListParameters) {

    // These need to be here because the API handles nulls but not empty strings well, so we
    // need to null-out anything that's an empty-string due to the angular two-way data binding to a text box
    if (params.TestSessionId === "") { params.TestSessionId = null; }
    if (params.VectorSetId === "") { params.VectorSetId = null; }

    var slightlyReformatted = {
      "page": params.page,
      "pageSize": params.pageSize,
      "vectorSetId": parseInt(params.VectorSetId),
      "testSessionId": parseInt(params.TestSessionId), // Because text inputs must bind to strings, but the API takes integers
      "testSessionStatus": (params.TestSessionStatus !== "All" && params.TestSessionStatus !== null) ? TestSessionStatus[params.TestSessionStatus] : null
    }

    return this.http.post<TestSessionList>(this.apiRoot + '/TestSessions', slightlyReformatted);
  }

  getTestSession(sessionId: number) {
    return this.http.get<TestSession>(this.apiRoot + '/TestSessions/' + sessionId);
  }

  getVectorSet(vectorSetId: number) {
    return this.http.get<VectorSet>(this.apiRoot + '/TestSessions/VectorSet/' + vectorSetId);
  }

  // END TestSession-related calls
}
