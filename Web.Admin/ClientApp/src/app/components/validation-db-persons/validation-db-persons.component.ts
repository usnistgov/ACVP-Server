import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { Person } from '../../models/Person/Person';
import { Router, ActivatedRoute } from '@angular/router';
import { PersonList } from '../../models/Person/PersonList';

@Component({
  selector: 'app-validation-db-persons',
  templateUrl: './validation-db-persons.component.html',
  styleUrls: ['./validation-db-persons.component.scss']
})
export class ValidationDbPersonsComponent implements OnInit {

  persons: PersonList;

  constructor(private ajs: AjaxService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {

    this.persons = new PersonList();
    this.persons.pageSize = 10;
    this.persons.currentPage = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.persons.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    this.ajs.getPersons(this.persons.pageSize, this.persons.currentPage).subscribe(
      data => { this.persons = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.persons.currentPage = 1;
    }
    else if (whichPage == "previous") {
      if (this.persons.currentPage > 1) {
        this.persons.currentPage = --this.persons.currentPage;
      }
    }
    else if (whichPage == "next") {
      if (this.persons.currentPage < this.persons.totalPages) {
        this.persons.currentPage = ++this.persons.currentPage;
      }
    }
    else if (whichPage == "last") {
      this.persons.currentPage = this.persons.totalPages;
    }

    this.ajs.getPersons(this.persons.pageSize, this.persons.currentPage).subscribe(
      data => {
        this.persons = data;
        this.router.navigate([], {
          queryParams: { page: this.persons.currentPage },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
