import { Component, OnInit } from '@angular/core';
import { ProductList } from '../../models/Product/ProductList';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductProviderService } from '../../services/ajax/product/product-provider.service';
import { ProductListParameters } from '../../models/Product/ProductListParameters';

@Component({
  selector: 'app-validation-db-products',
  templateUrl: './validation-db-products.component.html',
  styleUrls: ['./validation-db-products.component.scss']
})
export class ValidationDbProductsComponent implements OnInit {

  listData: ProductListParameters;
  products: ProductList;

  constructor(private ProductService: ProductProviderService, private router: Router, private route: ActivatedRoute) { }

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
    if (this.listData.description === "") {
      this.router.navigate([], {
        queryParams: { description: null },
        queryParamsHandling: 'merge'
      });
    }

    // Now, actually get the data
    this.ProductService.getProducts(this.listData).subscribe(
      data => {
        this.products = data;
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
    this.listData = new ProductListParameters("", "", "");
    this.products = new ProductList();

    this.listData.pageSize = 10;
    this.listData.page = 1;

    // Check if the page param is set.  If so, store it in the "currentPage"...
    if (this.route.snapshot.queryParamMap.get('page')) {
      this.listData.page = parseInt(this.route.snapshot.queryParamMap.get('page'));
    }
    if (this.route.snapshot.queryParamMap.get('name')) {
      this.listData.name = this.route.snapshot.queryParamMap.get('name');
    }

    this.ProductService.getProducts(this.listData).subscribe(
      data => { this.products = data; },
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
      if (this.listData.page < this.products.totalPages) {
        this.listData.page = ++this.listData.page;
      }
    }
    else if (whichPage == "last") {
      this.listData.page = this.products.totalPages;
    }

    this.ProductService.getProducts(this.listData).subscribe(
      data => {
        this.products = data;
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
