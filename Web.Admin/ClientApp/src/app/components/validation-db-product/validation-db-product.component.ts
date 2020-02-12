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

  constructor(private ajs: AjaxService, private route: ActivatedRoute, private modalService: ModalService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.ajs.getProduct(parseInt(params.get("id"))).subscribe(
        data => { this.selectedProduct = JSON.parse(JSON.stringify(data)); this.referenceCopyProduct = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { })
    });
  }

}
