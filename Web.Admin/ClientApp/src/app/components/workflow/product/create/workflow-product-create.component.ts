import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowProductCreatePayload } from '../../../../models/Workflow/Product/WorkflowProductCreatePayload';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { WorkflowCreateProductPayloadContact } from '../../../../models/Workflow/Product/WorkflowCreateProductPayloadContact';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-product-create',
  templateUrl: './workflow-product-create.component.html',
  styleUrls: ['./workflow-product-create.component.scss']
})
export class WorkflowProductCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowProductCreatePayload>;

  constructor(private ajs: AjaxService, private router: Router) { }

  approveWorkflow() {
    this.ajs.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }
  rejectWorkflow() {
    this.ajs.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
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
  set wfItem(item: WorkflowItemBase<WorkflowProductCreatePayload>) {
    this.workflowItem = item;

    this.ajs.getOrganization(this.workflowItem.payload.vendor.id).subscribe(
      data => { this.workflowItem.payload.vendor = JSON.parse(JSON.stringify(data)); },
      err => { },
      () => { }
    );

    for (let i = 0; i < this.workflowItem.payload.contacts.length; i++) {
      this.ajs.getPerson(this.workflowItem.payload.contacts[i].person.id).subscribe(
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

  }

  ngOnInit() {
  }

}
