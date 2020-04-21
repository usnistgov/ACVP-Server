import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowDeletePayload } from '../../../../models/workflow/WorkflowDeletePayload';
import { Organization } from '../../../../models/organization/Organization';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';

@Component({
  selector: 'app-workflow-organization-delete',
  templateUrl: './workflow-organization-delete.component.html',
  styleUrls: ['./workflow-organization-delete.component.scss']
})
export class WorkflowOrganizationDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  currentState: Organization;

  constructor(private OrganizationService: OrganizationProviderService) { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {

    // Set the workflow item data in place
    this.workflowItem = item;

    // Go get the current state for comparison
    this.OrganizationService.getOrganization(this.workflowItem.payload.id).subscribe(
      data => {
        this.currentState = data;
      },
      err => { },
      () => { }
    );

  }

  ngOnInit() {
  }

}
