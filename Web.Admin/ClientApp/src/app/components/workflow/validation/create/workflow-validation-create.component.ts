import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowValidationCreatePayload } from '../../../../models/workflow/validation/WorkflowValidationCreatePayload';

@Component({
  selector: 'app-workflow-validation-create',
  templateUrl: './workflow-validation-create.component.html',
  styleUrls: ['./workflow-validation-create.component.scss']
})
export class WorkflowValidationCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowValidationCreatePayload>;

  constructor() { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowValidationCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
