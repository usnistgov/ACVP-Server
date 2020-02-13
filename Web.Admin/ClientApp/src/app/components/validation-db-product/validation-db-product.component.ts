import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/Product/Product';
import { AjaxService } from '../../services/ajax/ajax.service';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../../services/modal/modal.service';

@Component({
  selector: 'app-validation-db-product',
  templateUrl: './validation-db-product.component.html',
  styleUrls: ['./validation-db-product.component.scss']
})
export class ValidationDbProductComponent implements OnInit {

  selectedProduct: Product;
  referenceCopyProduct: Product;
  updateStatusFlag = "none";
  updateAddressStatusFlag = "none";

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  updateProduct() {
    this.ajs.updateProduct(this.referenceCopyProduct).subscribe(
      data => { this.updateStatusFlag = "successful"; this.refreshPageData(); },
      err => { console.log("Update failed"); },
      () => { });
  }

  updateAddress() {
    this.ajs.updateAddress(this.referenceCopyProduct.address).subscribe(
      data => { this.updateAddressStatusFlag = "successful"; this.refreshPageData(); },
      err => { console.log("Address update failed"); },
      () => { });
  }

  refreshPageData() {
    this.ajs.getProduct(this.selectedProduct.id).subscribe(
      data => { this.selectedProduct = JSON.parse(JSON.stringify(data)); this.referenceCopyProduct = JSON.parse(JSON.stringify(data)); },
      err => { },
      () => { });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getProduct(parseInt(params.get("id"))).subscribe(
        data => { this.selectedProduct = JSON.parse(JSON.stringify(data)); this.referenceCopyProduct = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
