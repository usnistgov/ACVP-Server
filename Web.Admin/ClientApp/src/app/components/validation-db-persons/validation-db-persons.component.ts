import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { Person } from '../../models/Person/Person';

@Component({
  selector: 'app-validation-db-persons',
  templateUrl: './validation-db-persons.component.html',
  styleUrls: ['./validation-db-persons.component.scss']
})
export class ValidationDbPersonsComponent implements OnInit {

  persons: Person[];

  pageData = { "pageSize": 10, "pageNumber": 1 };

  constructor(private ajs: AjaxService) { }

  ngOnInit() {
    this.ajs.getPersons(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.persons = data.data; },
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

    this.ajs.getPersons(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.persons = data.data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
