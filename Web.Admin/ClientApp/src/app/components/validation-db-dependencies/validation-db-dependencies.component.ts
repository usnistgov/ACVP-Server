import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service'
import { DependencyLite } from '../../models/dependency-lite';

@Component({
  selector: 'app-validation-db-dependencies',
  templateUrl: './validation-db-dependencies.component.html',
  styleUrls: ['./validation-db-dependencies.component.scss']
})
export class ValidationDbDependenciesComponent implements OnInit {

  dependencies: DependencyLite[];

  pageData = { "pageSize" : 10, "pageNumber" : 1};

  constructor(private ajs: AjaxService) { }

  ngOnInit() {
    this.ajs.getDependencies(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.dependencies = data.data; console.log(this.dependencies); },
      err => { /* We should find something useful to do in here at some point.  Maybe a site-wide error popup in the HTML app.component? */},
      () => { }
    );
  }

}
