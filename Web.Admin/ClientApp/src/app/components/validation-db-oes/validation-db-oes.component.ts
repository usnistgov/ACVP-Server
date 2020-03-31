import { Component, OnInit } from '@angular/core';
import { OperatingEnvironmentList } from '../../models/OperatingEnvironment/OperatingEnvironmentList';
import { ActivatedRoute, Router } from '@angular/router';
import { OperatingEnvironmentProviderService } from '../../services/ajax/operatingEnvironment/operating-environment-provider.service';
import { OperatingEnvironmentListParameters } from '../../models/OperatingEnvironment/OperatingEnvironmentListParameters';

@Component({
  selector: 'app-validation-db-oes',
  templateUrl: './validation-db-oes.component.html',
  styleUrls: ['./validation-db-oes.component.scss']
})
export class ValidationDbOEsComponent implements OnInit {

  listData: OperatingEnvironmentListParameters;
  operatingEnvironments: OperatingEnvironmentList;

  constructor(private OEService: OperatingEnvironmentProviderService, private router: Router, private route: ActivatedRoute) { }

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
    this.OEService.getOEs(this.listData).subscribe(
      data => {
        this.operatingEnvironments = data;
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
    this.listData = new OperatingEnvironmentListParameters("", "");
    this.operatingEnvironments = new OperatingEnvironmentList();

    this.listData.pageSize = 10;
    this.listData.page = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('name')) {
      this.listData.name = this.route.snapshot.queryParamMap.get('name');
    }

    this.OEService.getOEs(this.listData).subscribe(
      data => { this.operatingEnvironments = data; },
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
      if (this.listData.page < this.operatingEnvironments.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.operatingEnvironments.totalPages;
    }

    this.OEService.getOEs(this.listData).subscribe(
      data => {
        this.operatingEnvironments = data;
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
