import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { OperatingEnvironment } from '../../models/operatingEnvironment/operatingEnvironment';

@Component({
  selector: 'app-validation-db-operating-environments',
  templateUrl: './validation-db-operating-environments.component.html',
  styleUrls: ['./validation-db-operating-environments.component.scss']
})
export class ValidationDbOperatingEnvironmentsComponent implements OnInit {

  operatingEnvironments: OperatingEnvironment[];

  pageData = { "pageSize": 10, "pageNumber": 1 };

  constructor(private ajs: AjaxService) { }

  ngOnInit() {
    this.ajs.getOEs(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.operatingEnvironments = data.data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.pageData.pageNumber = 1;
    }
    else if (whichPage == "previous") {
      this.pageData.pageNumber--;
    }
    else if (whichPage == "next") {
      this.pageData.pageNumber++;
    }

    this.ajs.getOEs(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.operatingEnvironments = data.data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
