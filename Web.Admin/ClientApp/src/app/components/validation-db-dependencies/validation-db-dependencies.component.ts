import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service'
import { DependencyLite } from '../../models/dependency/dependency-lite';

@Component({
  selector: 'app-validation-db-dependencies',
  templateUrl: './validation-db-dependencies.component.html',
  styleUrls: ['./validation-db-dependencies.component.scss']
})
export class ValidationDbDependenciesComponent implements OnInit {

  dependencies: DependencyLite[];

  pageData = { "pageSize" : 10, "pageNumber" : 1 };

  constructor(private ajs: AjaxService) { }

  ngOnInit() {
    this.ajs.getDependencies(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.dependencies = data.data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */},
      () => { }
    );
  }

  getPage(whichPage:string) {

    if (whichPage == "first") {
      this.pageData.pageNumber = 1;
    }
    else if (whichPage == "previous") {
      this.pageData.pageNumber--;
    }
    else if (whichPage == "next") {
      this.pageData.pageNumber++;
    }

    this.ajs.getDependencies(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.dependencies = data.data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
