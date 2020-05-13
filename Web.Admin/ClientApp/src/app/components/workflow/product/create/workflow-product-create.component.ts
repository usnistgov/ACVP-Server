import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowProductCreatePayload } from '../../../../models/workflow/product/WorkflowProductCreatePayload';
import { WorkflowCreateProductPayloadContact } from '../../../../models/workflow/product/WorkflowCreateProductPayloadContact';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';
import { PersonProviderService } from '../../../../services/ajax/person/person-provider.service';

@Component({
  selector: 'app-workflow-product-create',
  templateUrl: './workflow-product-create.component.html',
  styleUrls: ['./workflow-product-create.component.scss']
})
export class WorkflowProductCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowProductCreatePayload>;

  constructor(private PersonService: PersonProviderService,
    private OrganizationService: OrganizationProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowProductCreatePayload>) {
    this.workflowItem = item;

    if (this.workflowItem.payload.vendorUrl !== null) {
      let vendorID = parseInt(this.workflowItem.payload.vendorUrl.split('/')[this.workflowItem.payload.vendorUrl.split('/').length - 1]);
      this.OrganizationService.getOrganization(vendorID).subscribe(
        data => { this.workflowItem.payload.vendor = JSON.parse(JSON.stringify(data)); },
        err => { },
        () => { }
      );
    }

    // Intialize the contacts array, otherwise it's null
    this.workflowItem.payload.contacts = [];

    // Now, loop through the URLs in the list and GET each one, storing it in the newly-minted list
    if (this.workflowItem.payload.contactUrls !== null) {
      for (let i = 0; i < this.workflowItem.payload.contactUrls.length; i++) {

        // Parse out the person id
        let personId = parseInt(this.workflowItem.payload.contactUrls[i].split("/")[this.workflowItem.payload.contactUrls[i].split("/").length - 1]);

        // Now GET the person record
        this.PersonService.getPerson(personId).subscribe(
          data => {
            let contact = new WorkflowCreateProductPayloadContact();
            contact.person = JSON.parse(JSON.stringify(data));
            contact.orderIndex = i;
            this.workflowItem.payload.contacts[i] = contact;
          },
          err => { },
          () => { }
        );
      }

      if (this.workflowItem.payload.addressUrl !== null && this.workflowItem.payload.addressUrl !== "") {

        let addressId = parseInt(this.workflowItem.payload.addressUrl.split("/")[this.workflowItem.payload.addressUrl.split("/").length - 1]);

        this.PersonService.getAddress(addressId).subscribe(
          data => {
            this.workflowItem.payload.address = data;
          }
        );
      }
    }

  }

  ngOnInit() {
  }

}
