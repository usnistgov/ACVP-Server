import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { WorkflowDeletePayload } from '../../../../models/workflow/WorkflowDeletePayload';
import { OperatingEnvironment } from '../../../../models/operatingEnvironment/OperatingEnvironment';
import { OperatingEnvironmentProviderService } from '../../../../services/ajax/operatingEnvironment/operating-environment-provider.service';

@Component({
  selector: 'app-workflow-oe-delete',
  templateUrl: './workflow-oe-delete.component.html',
  styleUrls: ['./workflow-oe-delete.component.scss']
})
export class WorkflowOeDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  currentState: OperatingEnvironment;
  objectKeys = Object.keys;

  constructor(private OEService: OperatingEnvironmentProviderService) { }

  shouldAttributeBeDisplayed(key: string) {
    if (key == "id") {
      return false;
    }
    return true;
  }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {
    this.workflowItem = item;

    this.OEService.getOE(this.workflowItem.payload.id).subscribe(
      data => {
        this.currentState = data;
      },
      err => { },
      () => { }
    );
  }

  ngOnInit() {
  }

}
