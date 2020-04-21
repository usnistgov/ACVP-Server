import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { OperatingEnvironmentCreatePayload } from '../../../../models/workflow/operatingEnvironment/OperatingEnvironmentCreatePayload';

@Component({
  selector: 'app-workflow-oe-create',
  templateUrl: './workflow-oe-create.component.html',
  styleUrls: ['./workflow-oe-create.component.scss']
})
export class WorkflowOeCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<OperatingEnvironmentCreatePayload>;
  objectKeys = Object.keys;

  constructor() { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<OperatingEnvironmentCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
