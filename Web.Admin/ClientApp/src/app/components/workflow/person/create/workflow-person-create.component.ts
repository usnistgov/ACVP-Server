import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowPersonCreatePayload } from '../../../../models/Workflow/Person/WorkflowPersonCreatePayload';
import { AjaxService } from '../../../../services/ajax/ajax.service';

@Component({
  selector: 'app-workflow-person-create',
  templateUrl: './workflow-person-create.component.html',
  styleUrls: ['./workflow-person-create.component.scss']
})
export class WorkflowPersonCreateComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowPersonCreatePayload>;

  constructor() { }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowPersonCreatePayload>) {
    this.workflowItem = item;
  }

  ngOnInit() {
  }

}
