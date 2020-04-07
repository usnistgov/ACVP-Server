import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowProductUpdatePayload } from '../../../../models/workflow/product/WorkflowProductUpdatePayload';
import { Product } from '../../../../models/product/Product';
import { Router } from '@angular/router';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
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

  constructor(private ProductService: ProductProviderService, private PersonService: PersonProviderService, private OrganizationService: OrganizationProviderService, private workflowService: WorkflowProviderService, private router: Router) { }

  approveWorkflow() {
    this.workflowService.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }
  rejectWorkflow() {
    this.workflowService.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }

  refreshPageData() {
    this.router.navigateByUrl('/', { skipLocationChange: true })
      .then(() =>
        this.router.navigate(['workflow/' + this.workflowItem.workflowItemID])
      );
  }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowProductUpdatePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Get the data for the vendor listed in the WF item
    this.OrganizationService.getOrganization(this.workflowItem.payload.vendor.id).subscribe(
      data => { this.workflowItem.payload.vendor = data; }
    );

    // Get the data for each contact in the contacts list
    for (let i = 0; i < this.workflowItem.payload.contacts.length; i++) {
      this.PersonService.getPerson(this.workflowItem.payload.contacts[i].person.id).subscribe(
        data => { this.workflowItem.payload.contacts[i].person = data; }
      );
    }

    // Get the current state
    this.ProductService.getProduct(this.workflowItem.payload.id).subscribe(
      data => {

        this.currentState = data;

        // Get the data for the vendor listed in the currentState
        this.OrganizationService.getOrganization(this.workflowItem.payload.vendor.id).subscribe(
          data => {
            this.workflowItem.payload.vendor = data;
            console.log(data);
          }
        );

      }
    );

  }

  ngOnInit() {
  }

}
