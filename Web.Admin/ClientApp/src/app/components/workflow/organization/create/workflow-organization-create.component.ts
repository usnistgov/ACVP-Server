import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowOrganizationCreatePayload } from '../../../../models/workflow/organization/WorkflowOrganizationCreatePayload';

@Component({
  selector: 'app-workflow-organization-create',
  templateUrl: './workflow-organization-create.component.html',
  styleUrls: ['./workflow-organization-create.component.scss']
})
export class WorkflowOrganizationCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowOrganizationCreatePayload>;

  constructor() { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowOrganizationCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
