import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';

@Component({
  selector: 'app-workflow-organization-create',
  templateUrl: './workflow-organization-create.component.html',
  styleUrls: ['./workflow-organization-create.component.scss']
})
export class WorkflowOrganizationCreateComponent implements OnInit {

  private workflowItem: WorkflowItemBase;

  constructor() { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
