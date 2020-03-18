import { Component, OnInit } from '@angular/core';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ProductList } from '../../models/Product/ProductList';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-validation-db-products',
  templateUrl: './validation-db-products.component.html',
  styleUrls: ['./validation-db-products.component.scss']
})
export class ValidationDbProductsComponent implements OnInit {

  products: ProductList;

  constructor(private ajs: AjaxService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {

    this.products = new ProductList();
    this.products.pageSize = 10;
    this.products.currentPage = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.products.currentPage = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }

    this.ajs.getProducts(this.products.pageSize, this.products.currentPage).subscribe(
      data => { this.products = data; },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  }

  getPage(whichPage: string) {

    if (whichPage == "first") {
      this.products.currentPage = 1;
    }
    else if (whichPage == "previous") {
      if (this.products.currentPage > 1) {
        this.products.currentPage = --this.products.currentPage;
      }
    }
    else if (whichPage == "next") {
      if (this.products.currentPage < this.products.totalPages) {
        this.products.currentPage = ++this.products.currentPage;
      }
    }
    else if (whichPage == "last") {
      this.products.currentPage = this.products.totalPages;
    }

    this.ajs.getProducts(this.products.pageSize, this.products.currentPage).subscribe(
      data => {
        this.products = data;
        this.router.navigate([], {
          queryParams: { page: this.products.currentPage },
          queryParamsHandling: 'merge'
        });
      },
      err => { /* we should find something useful to do in here at some point.  maybe a site-wide error popup in the html app.component? */ },
      () => { }
    );
  };

}
