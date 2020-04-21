import { Injectable } from '@angular/core';
import { AcvpUserList } from '../../../models/AcvpUser/AcvpUserList';
import { HttpClient } from '@angular/common/http';
import { AcvpUserListParameters } from '../../../models/AcvpUser/AcvpUserListParameters';
import { AcvpUser } from '../../../models/AcvpUser/AcvpUser';
import { AcvpUserCreateParameters } from '../../../models/AcvpUser/AcvpUserCreateParameters';
import { Result } from '../../../models/responses/Result';
import { AcvpUserSeedUpdateParameters } from '../../../models/AcvpUser/AcvpUserSeedUpdateParameters';
import { AcvpUserCertificateUpdateParameters } from '../../../models/AcvpUser/AcvpUserCertificateUpdateParameters';

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

  updateSeed(userId: number, param: AcvpUserSeedUpdateParameters) {
    return this.http.post<Result>(this.apiRoot + '/Users/' +  userId + '/seed', param);
  }

  refreshSeed(userId: number) {
    return this.http.post<Result>(this.apiRoot + '/Users/' + userId + '/seed/refresh', {});
  }

  updateCertificate(userId: number, param: AcvpUserCertificateUpdateParameters) {
    return this.http.post<Result>(this.apiRoot + '/Users/' + userId + '/certificate', param);
  }
}
