import { Component, OnInit } from '@angular/core';
import { OrganizationLite } from '../../models/Organization/OrganizationLite';
import { AjaxService } from '../../services/ajax/ajax.service';
import { OrganizationList } from '../../models/Organization/OrganizationList';
import { PagingService } from '../../services/paging/paging.service';

@Component({
  selector: 'app-validation-db-organizations',
  templateUrl: './validation-db-organizations.component.html',
  styleUrls: ['./validation-db-organizations.component.scss']
})
export class ValidationDbOrganizationsComponent implements OnInit {

  organizations: OrganizationList;

  constructor(private ajs: AjaxService, private paging: PagingService) { }

  ngOnInit() {
    this.organizations = new OrganizationList();
    this.organizations.currentPage = 1;
    this.organizations.pageSize = 10;
    this.ajs.getOrganizations(this.organizations.pageSize, this.organizations.currentPage).subscribe(
      data => { this.organizations = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.organizations.currentPage = 1;
    }
    else if (whichPage == "previous") {
      this.organizations.currentPage--;
    }
    else if (whichPage == "next") {
      this.organizations.currentPage++;
    }
    else if (whichPage == "last") {
      this.organizations.currentPage = this.organizations.totalPages;
    }

    this.ajs.getOrganizations(this.organizations.pageSize, this.organizations.currentPage).subscribe(
      data => { this.organizations = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
