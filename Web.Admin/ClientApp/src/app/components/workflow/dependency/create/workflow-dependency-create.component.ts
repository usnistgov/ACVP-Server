import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';

@Component({
  selector: 'app-workflow-dependency-create',
  templateUrl: './workflow-dependency-create.component.html',
  styleUrls: ['./workflow-dependency-create.component.scss']
})
export class WorkflowDependencyCreateComponent implements OnInit {

  constructor() { }

  private workflowItem: WorkflowItemBase;

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
