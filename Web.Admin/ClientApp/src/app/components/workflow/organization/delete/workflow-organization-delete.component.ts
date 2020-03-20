import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDeletePayload } from '../../../../models/Workflow/WorkflowDeletePayload';
import { Organization } from '../../../../models/Organization/Organization';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-organization-delete',
  templateUrl: './workflow-organization-delete.component.html',
  styleUrls: ['./workflow-organization-delete.component.scss']
})
export class WorkflowOrganizationDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  currentState: Organization;

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
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Go get the current state for comparison
    this.ajs.getOrganization(this.workflowItem.payload.id).subscribe(
      data => {
        this.currentState = data;
        console.log(data);
      },
      err => { },
      () => { }
    );

  }

  ngOnInit() {
  }

}
