import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { OrganizationList } from '../../models/Organization/OrganizationList';


@Injectable({
  providedIn: 'root'
})
export class PagingService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  getPage<IWrappedEnumerable>(currentPageData: IWrappedEnumerable, whichPage: string) {

    console.log(currentPageData.constructor.name);

    if (currentPageData instanceof OrganizationList) {
      console.log("MARKER 2");
      return this.http.post(this.apiRoot + '/Organizations', currentPageData);
    }
    
  };
}
