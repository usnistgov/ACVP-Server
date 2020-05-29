import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PersonList } from '../../models/person/PersonList';
import { PersonProviderService } from '../../services/ajax/person/person-provider.service';
import { PersonListParameters } from '../../models/person/PersonListParameters';

@Component({
  selector: 'app-validation-db-persons',
  templateUrl: './validation-db-persons.component.html',
  styleUrls: ['./validation-db-persons.component.scss']
})
export class ValidationDbPersonsComponent implements OnInit {

  listData: PersonListParameters;
  persons: PersonList;

  constructor(private PersonService: PersonProviderService, private router: Router, private route: ActivatedRoute) { }

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
    if (this.listData.organizationName === "") {
      this.router.navigate([], {
        queryParams: { organizationName: null },
        queryParamsHandling: 'merge'
      });
    }

    // Now, actually get the data
    this.PersonService.getPersons(this.listData).subscribe(
      data => {
        this.persons = data;
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
    this.listData = new PersonListParameters("", "", "");
    this.persons = new PersonList();

    this.listData.pageSize = 10;
    this.listData.page = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('name')) {
      this.listData.name = this.route.snapshot.queryParamMap.get('name');
    }

    this.PersonService.getPersons(this.listData).subscribe(
      data => { this.persons = data; },
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
      if (this.listData.page < this.persons.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.persons.totalPages;
    }

    this.PersonService.getPersons(this.listData).subscribe(
      data => {
        this.persons = data;
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
