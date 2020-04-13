import { Injectable } from '@angular/core';
import { AcvpUserList } from '../../../models/AcvpUser/AcvpUserList';
import { HttpClient } from '@angular/common/http';
import { AcvpUserListParameters } from '../../../models/AcvpUser/AcvpUserListParameters';
import { AcvpUser } from '../../../models/AcvpUser/AcvpUser';
import { AcvpUserCreateParameters } from '../../../models/AcvpUser/AcvpUserCreateParameters';
import { Result } from '../../../models/responses/Result';

@Injectable({
  providedIn: 'root'
})
export class AcvpUserDataProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  getAcvpUsers(params: AcvpUserListParameters) {
    return this.http.post<AcvpUserList>(this.apiRoot + '/Users', params);
  }

  getAcvpUser(id: number) {
    return this.http.get<AcvpUser>(this.apiRoot + '/Users/' + id);
  }

  createAcvpUser(params: AcvpUserCreateParameters) {
    return this.http.put<Result>(this.apiRoot + '/Users', params);
  }

  deleteAcvpUser(id: number) {
    return this.http.delete<Result>(this.apiRoot + '/Users/' + id);
  }
}
