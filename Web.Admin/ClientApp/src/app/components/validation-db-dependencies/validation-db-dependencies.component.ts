import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service'
import { DependencyList } from '../../models/dependency/dependency-list';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-validation-db-dependencies',
  templateUrl: './validation-db-dependencies.component.html',
  styleUrls: ['./validation-db-dependencies.component.scss']
})
export class ValidationDbDependenciesComponent implements OnInit {

  dependencies: DependencyList;

  constructor(private ajs: AjaxService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {

    this.dependencies = new DependencyList();
    this.dependencies.pageSize = 10;
    this.dependencies.currentPage = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.dependencies.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    this.ajs.getDependencies(this.dependencies.pageSize, this.dependencies.currentPage).subscribe(
      data => { this.dependencies = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */},
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.dependencies.currentPage = 1;
    }
    else if (whichPage == "previous") {
      if (this.dependencies.currentPage > 1) {
        this.dependencies.currentPage = --this.dependencies.currentPage;
      }
    }
    else if (whichPage == "next") {
      if (this.dependencies.currentPage < this.dependencies.totalPages) {
        this.dependencies.currentPage = ++this.dependencies.currentPage;
      }
    }
    else if (whichPage == "last") {
      this.dependencies.currentPage = this.dependencies.totalPages;
    }

    this.ajs.getDependencies(this.dependencies.pageSize, this.dependencies.currentPage).subscribe(
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
  };

}
