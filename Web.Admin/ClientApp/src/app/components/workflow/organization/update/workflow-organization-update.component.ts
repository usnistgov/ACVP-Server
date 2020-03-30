import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { Router } from '@angular/router';
import { WorkflowOrganizationUpdatePayload } from '../../../../models/Workflow/Organization/WorkflowOrganizationUpdatePayload';
import { Organization } from '../../../../models/Organization/Organization';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';

@Component({
  selector: 'app-workflow-organization-update',
  templateUrl: './workflow-organization-update.component.html',
  styleUrls: ['./workflow-organization-update.component.scss']
})
export class WorkflowOrganizationUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowOrganizationUpdatePayload>;
  currentState: Organization;

  constructor(private OrganizationService: OrganizationProviderService, private workflowService: WorkflowProviderService, private router: Router) { }

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
  set wfItem(item: WorkflowItemBase<WorkflowOrganizationUpdatePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Go get the current state for comparison
    this.OrganizationService.getOrganization(this.workflowItem.payload.id).subscribe(
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
