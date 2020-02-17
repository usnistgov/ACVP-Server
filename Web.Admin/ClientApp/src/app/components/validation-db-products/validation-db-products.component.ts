import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { Product } from '../../models/Product/Product';

@Component({
  selector: 'app-validation-db-products',
  templateUrl: './validation-db-products.component.html',
  styleUrls: ['./validation-db-products.component.scss']
})
export class ValidationDbProductsComponent implements OnInit {

  products: Product[];

  pageData = { "pageSize": 10, "pageNumber": 1 };

  constructor(private ajs: AjaxService) { }

  ngOnInit() {
    this.ajs.getProducts(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.products = data.data; },
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

    this.ajs.getProducts(this.pageData.pageSize, this.pageData.pageNumber).subscribe(
      data => { this.products = data.data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
