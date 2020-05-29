import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowDependencyCreatePayload } from '../../../../models/workflow/dependency/WorkflowDependencyCreatePayload';

@Component({
  selector: 'app-workflow-dependency-create',
  templateUrl: './workflow-dependency-create.component.html',
  styleUrls: ['./workflow-dependency-create.component.scss']
})
export class WorkflowDependencyCreateComponent implements OnInit {

  constructor() { }

  workflowItem: WorkflowItemBase<WorkflowDependencyCreatePayload>;

  /*
   * This is how the component takes the workflowItem from the main workflow controller using the
   * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
   */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDependencyCreatePayload>) {
    this.workflowItem = item;
  }

  // This is simply a utility function used to return a form of the attributes with the non-user-defined ones out, as well
  // as the ones related to ACVP UI, like id, url, etc.
  filterKeys() {
    return Object.keys(this.workflowItem.payload).filter(function (element, index, array) {
      if (element === "id" ||
        element === "name" ||
        element === "type" ||
        element === "description" ||
        element === "url") {
        return false;
      } else {
        return true;
      }
    });
  }

  ngOnInit() {
  }

}
