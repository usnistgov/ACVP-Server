import { Component, OnInit, Input } from '@angular/core';
import { WorkflowDeletePayload } from '../../../../models/workflow/WorkflowDeletePayload';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { Product } from '../../../../models/product/Product';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';
import { ProductProviderService } from '../../../../services/ajax/product/product-provider.service';

@Component({
  selector: 'app-workflow-product-delete',
  templateUrl: './workflow-product-delete.component.html',
  styleUrls: ['./workflow-product-delete.component.scss']
})
export class WorkflowProductDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  product: Product;

  constructor(private ProductService: ProductProviderService, private OrganizationService: OrganizationProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {
    this.workflowItem = item;

    this.ProductService.getProduct(this.workflowItem.payload.id).subscribe(
      data => {
        this.product = JSON.parse(JSON.stringify(data));

        this.OrganizationService.getOrganization(this.product.vendor.id).subscribe(
          data => { this.product.vendor = JSON.parse(JSON.stringify(data)); },
          err => { },
          () => { }
        );

      },
      err => { },
      () => { }
    );

  }

  ngOnInit() {
  }

}
