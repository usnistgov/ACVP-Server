import { Component, OnInit } from '@angular/core';
import { OrganizationList } from '../../models/organization/OrganizationList';
import { OrganizationProviderService } from '../../services/ajax/organization/organization-provider.service';
import { OrganizationListParameters } from '../../models/organization/OrganizationListParameters';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-validation-db-organizations',
  templateUrl: './validation-db-organizations.component.html',
  styleUrls: ['./validation-db-organizations.component.scss']
})
export class ValidationDbOrganizationsComponent implements OnInit {

  listData: OrganizationListParameters;
  organizations: OrganizationList;

  constructor(private OrganizationService: OrganizationProviderService, private router: Router, private route: ActivatedRoute) { }

  loadData() {

    // Anytime the user's search changes, we default to page one
    this.listData.page = 1;

    // This sets the queryParams, but if they're empty, they end up having "&name=" by itself in the URL
    // So the following if statements check each of the available routeParams and clear them from the URL if they're set
    this.router.navigate([], {
      queryParams: this.listData
    });

    // Clear empty ones as necessary
    if (this.listData.name === "") {
      this.router.navigate([], {
        queryParams: { name: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.id === "") {
      this.router.navigate([], {
        queryParams: { id: null },
        queryParamsHandling: 'merge'
      });
    }

    // Now, actually get the data
    this.OrganizationService.getOrganizations(this.listData).subscribe(
      data => {
        this.organizations = data;
        this.router.navigate([], {
          queryParams: { page: this.listData.page },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  ngOnInit() {
    this.listData = new OrganizationListParameters("", "");
    this.organizations = new OrganizationList();

    this.listData.pageSize = 10;
    this.listData.page = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('name')) {
      this.listData.name = this.route.snapshot.queryParamMap.get('name');
    }

    this.OrganizationService.getOrganizations(this.listData).subscribe(
      data => { this.organizations = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.listData.page = 1;
    }
    else if (whichPage == "previous") {
      if (this.listData.page > 1) {
        this.listData.page = --this.listData.page;
      }
    }
    else if (whichPage == "next") {
      if (this.listData.page < this.organizations.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.organizations.totalPages;
    }

    this.OrganizationService.getOrganizations(this.listData).subscribe(
      data => {
        this.organizations = data;
        this.router.navigate([], {
          queryParams: { page: this.listData.page },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
