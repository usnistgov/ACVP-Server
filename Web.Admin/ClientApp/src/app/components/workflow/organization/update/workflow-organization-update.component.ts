import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowOrganizationUpdatePayload } from '../../../../models/workflow/organization/WorkflowOrganizationUpdatePayload';
import { Organization } from '../../../../models/organization/Organization';
import { OrganizationProviderService } from '../../../../services/ajax/organization/organization-provider.service';

@Component({
  selector: 'app-workflow-organization-update',
  templateUrl: './workflow-organization-update.component.html',
  styleUrls: ['./workflow-organization-update.component.scss']
})
export class WorkflowOrganizationUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowOrganizationUpdatePayload>;
  currentState: Organization;

  constructor(private OrganizationService: OrganizationProviderService) { }

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
