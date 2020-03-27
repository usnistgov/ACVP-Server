import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service'
import { DependencyList } from '../../models/dependency/dependency-list';
import { Router, ActivatedRoute } from '@angular/router';
import { DependencyDataProviderService } from '../../services/ajax/dependency/dependency-data-provider.service';
import { DependencyListParameters } from '../../models/dependency/DependencyListParameters';

@Component({
  selector: 'app-validation-db-dependencies',
  templateUrl: './validation-db-dependencies.component.html',
  styleUrls: ['./validation-db-dependencies.component.scss']
})
export class ValidationDbDependenciesComponent implements OnInit {

  dependencies: DependencyList;
  listData: DependencyListParameters;

  constructor(private dds: DependencyDataProviderService, private router: Router, private route: ActivatedRoute) { }

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
    if (this.listData.type === "") {
      this.router.navigate([], {
        queryParams: { type: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.description === "") {
      this.router.navigate([], {
        queryParams: { description: null },
        queryParamsHandling: 'merge'
      });
    }

    // Now, actually get the data
    this.dds.getDependencies(this.listData).subscribe(
      data => {
        this.dependencies = data;
        this.router.navigate([], {
          queryParams: { page: this.dependencies.currentPage },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  ngOnInit() {
    this.listData = new DependencyListParameters("","","");
    this.dependencies = new DependencyList();

    this.listData.pageSize = 10;
    this.listData.page = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('name')) {
      this.listData.name = this.route.snapshot.queryParamMap.get('name');
    }
    if (this.route.snapshot.queryParamMap.get('type')) {
      this.listData.type = this.route.snapshot.queryParamMap.get('type');
    }
    if (this.route.snapshot.queryParamMap.get('description')) {
      this.listData.description = this.route.snapshot.queryParamMap.get('description');
    }

    this.dds.getDependencies(this.listData).subscribe(
      data => { this.dependencies = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */},
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
      if (this.listData.page < this.dependencies.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.dependencies.totalPages;
    }

    this.dds.getDependencies(this.listData).subscribe(
      data => {
        this.dependencies = data;
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
