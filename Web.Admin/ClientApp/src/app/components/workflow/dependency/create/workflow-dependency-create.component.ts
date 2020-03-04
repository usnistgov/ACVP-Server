import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDependencyCreatePayload } from '../../../../models/Workflow/Dependency/WorkflowDependencyCreatePayload';

@Component({
  selector: 'app-workflow-dependency-create',
  templateUrl: './workflow-dependency-create.component.html',
  styleUrls: ['./workflow-dependency-create.component.scss']
})
export class WorkflowDependencyCreateComponent implements OnInit {

  constructor() { }

  workflowItem: WorkflowItemBase<WorkflowDependencyCreatePayload>;
  objectKeys = Object.keys;

  /*
   * This is how the component takes the workflowItem from the main workflow controller using the
   * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
   */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDependencyCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
