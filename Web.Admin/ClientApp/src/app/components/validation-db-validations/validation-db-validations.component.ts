import { Component, OnInit } from '@angular/core';
import { ValidationList } from '../../models/validation/ValidationList';
import { ValidationListParameters } from '../../models/validation/ValidationListParameters';
import { Router, ActivatedRoute } from '@angular/router';
import { ValidationProviderService } from '../../services/ajax/validation/validation-provider.service';

@Component({
  selector: 'app-validation-db-validations',
  templateUrl: './validation-db-validations.component.html',
  styleUrls: ['./validation-db-validations.component.scss']
})
export class ValidationDbValidationsComponent implements OnInit {

  listData: ValidationListParameters;
  validations: ValidationList;

  constructor(private ValidationService: ValidationProviderService, private router: Router, private route: ActivatedRoute) { }

  loadData() {

    // Anytime the user's search changes, we default to page one
    this.listData.page = 1;

    // This sets the queryParams, but if they're empty, they end up having "&name=" by itself in the URL
    // So the following if statements check each of the available routeParams and clear them from the URL if they're set
    this.router.navigate([], {
      queryParams: this.listData
    });

    // Clear empty ones as necessary
    if (this.listData.productName === "") {
      this.router.navigate([], {
        queryParams: { productName: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.validationId === "") {
      this.router.navigate([], {
        queryParams: { validationId: null },
        queryParamsHandling: 'merge'
      });
    }
    if (this.listData.validationLabel === "") {
      this.router.navigate([], {
        queryParams: { validationId: null },
        queryParamsHandling: 'merge'
      });
    }

    // Now, actually get the data
    this.ValidationService.getValidations(this.listData).subscribe(
      data => {
        this.validations = data;
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
    this.listData = new ValidationListParameters("", "", "");
    this.validations = new ValidationList();

    this.listData.pageSize = 10;
    this.listData.page = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('productName')) {
      this.listData.productName = this.route.snapshot.queryParamMap.get('name');
    }
    if (this.route.snapshot.queryParamMap.get('validationId')) {
      this.listData.validationId = this.route.snapshot.queryParamMap.get('name');
    }
    if (this.route.snapshot.queryParamMap.get('validationLabel')) {
      this.listData.validationLabel = this.route.snapshot.queryParamMap.get('name');

    }
    this.ValidationService.getValidations(this.listData).subscribe(
      data => { this.validations = data; },
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
      if (this.listData.page < this.validations.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.validations.totalPages;
    }

    this.ValidationService.getValidations(this.listData).subscribe(
      data => {
        this.validations = data;
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
