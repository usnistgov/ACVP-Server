import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDependencyUpdatePayload } from '../../../../models/Workflow/Dependency/WorkflowDependencyUpdatePayload';

@Component({
  selector: 'app-workflow-dependency-update',
  templateUrl: './workflow-dependency-update.component.html',
  styleUrls: ['./workflow-dependency-update.component.scss']
})
export class WorkflowDependencyUpdateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDependencyUpdatePayload>;

  constructor() { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDependencyUpdatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
