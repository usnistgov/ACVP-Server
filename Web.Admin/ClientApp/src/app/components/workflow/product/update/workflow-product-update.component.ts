import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowProductUpdatePayload } from '../../../../models/workflow/product/WorkflowProductUpdatePayload';
import { Product } from '../../../../models/product/Product';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';
import { ProductProviderService } from '../../../../services/ajax/product/product-provider.service';
import { PersonProviderService } from '../../../../services/ajax/person/person-provider.service';

@Component({
  selector: 'app-workflow-product-update',
  templateUrl: './workflow-product-update.component.html',
  styleUrls: ['./workflow-product-update.component.scss']
})
export class WorkflowProductUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowProductUpdatePayload>;
  currentState: Product;

  constructor(private ProductService: ProductProviderService, private PersonService: PersonProviderService, private OrganizationService: OrganizationProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowProductUpdatePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Get the data for the vendor listed in the WF item
    if (this.workflowItem.payload.vendorUrl !== null) {
      let vendorID = parseInt(this.workflowItem.payload.vendorUrl.split('/')[this.workflowItem.payload.vendorUrl.split('/').length - 1]);
      this.OrganizationService.getOrganization(vendorID).subscribe(
        data => { this.workflowItem.payload.vendor = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { }
      );
    }

    // Get the data for each contact in the contacts list
    if (this.workflowItem.payload.contactUrls !== null){
      for (let i = 0; i < this.workflowItem.payload.contactUrls.length; i++) {
        let personID = parseInt(this.workflowItem.payload.contactUrls[i].split('/')[this.workflowItem.payload.contactUrls[i].split('/').length - 1]);
        this.PersonService.getPerson(personID).subscribe(
          data => { this.workflowItem.payload.contacts[i].person = data; }
        );
      }
    }
    
    // Get the current state
    this.ProductService.getProduct(this.workflowItem.payload.id).subscribe(
      data => {
        this.currentState = data;
      }
    );

  }

  ngOnInit() {
  }

}
