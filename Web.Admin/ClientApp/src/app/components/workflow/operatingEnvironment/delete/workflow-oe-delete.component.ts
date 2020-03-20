import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDeletePayload } from '../../../../models/Workflow/WorkflowDeletePayload';
import { OperatingEnvironment } from '../../../../models/operatingEnvironment/operatingEnvironment';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-oe-delete',
  templateUrl: './workflow-oe-delete.component.html',
  styleUrls: ['./workflow-oe-delete.component.scss']
})
export class WorkflowOeDeleteComponent implements OnInit {

  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  currentState: OperatingEnvironment;
  objectKeys = Object.keys;

  constructor(private ajs: AjaxService, private router: Router) { }

  shouldAttributeBeDisplayed(key: string) {
    if (key == "id") {
      return false;
    }
    return true;
  }

  approveWorkflow() {
    this.ajs.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }
  rejectWorkflow() {
    this.ajs.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }

  refreshPageData() {
    this.router.navigateByUrl('/', { skipLocationChange: true })
      .then(() =>
        this.router.navigate(['workflow/' + this.workflowItem.workflowItemID])
      );
  }

  /*
 * This is how the component takes the workflowItem from the main workflow controller using the
 * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
 */
  @Input()
  set wfItem(item: WorkflowItemBase<WorkflowDeletePayload>) {
    this.workflowItem = item;

    this.ajs.getOE(this.workflowItem.payload.id).subscribe(
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
