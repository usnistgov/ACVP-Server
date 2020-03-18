import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { OperatingEnvironmentList } from '../../models/OperatingEnvironment/OperatingEnvironmentList';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-validation-db-oes',
  templateUrl: './validation-db-oes.component.html',
  styleUrls: ['./validation-db-oes.component.scss']
})
export class ValidationDbOEsComponent implements OnInit {

  operatingEnvironments: OperatingEnvironmentList;

  constructor(private ajs: AjaxService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {

    this.operatingEnvironments = new OperatingEnvironmentList();
    this.operatingEnvironments.pageSize = 10;
    this.operatingEnvironments.currentPage = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.operatingEnvironments.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    this.ajs.getOEs(this.operatingEnvironments.pageSize, this.operatingEnvironments.currentPage).subscribe(
      data => { this.operatingEnvironments = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.operatingEnvironments.currentPage = 1;
    }
    else if (whichPage == "previous") {
      if (this.operatingEnvironments.currentPage > 1) {
        this.operatingEnvironments.currentPage = --this.operatingEnvironments.currentPage;
      }
    }
    else if (whichPage == "next") {
      if (this.operatingEnvironments.currentPage < this.operatingEnvironments.totalPages) {
        this.operatingEnvironments.currentPage = ++this.operatingEnvironments.currentPage;
      }
    }
    else if (whichPage == "last") {
      this.operatingEnvironments.currentPage = this.operatingEnvironments.totalPages;
    }

    this.ajs.getOEs(this.operatingEnvironments.pageSize, this.operatingEnvironments.currentPage).subscribe(
      data => {
        this.operatingEnvironments = data;
        this.router.navigate([], {
          queryParams: { page: this.operatingEnvironments.currentPage },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
