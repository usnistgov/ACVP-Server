import { Component, OnInit, Input } from '@angular/core';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDeletePayload } from '../../../../models/Workflow/WorkflowDeletePayload';
import { Dependency } from '../../../../models/dependency/dependency';

@Component({
  selector: 'app-workflow-dependency-delete',
  templateUrl: './workflow-dependency-delete.component.html',
  styleUrls: ['./workflow-dependency-delete.component.scss']
})
export class WorkflowDependencyDeleteComponent implements OnInit {

  currentState: Dependency;
  workflowItem: WorkflowItemBase<WorkflowDeletePayload>;
  objectKeys = Object.keys;

  constructor(private ajs: AjaxService, private router: Router) { }

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

    this.ajs.getDependency(this.workflowItem.payload.id).subscribe(
      data => { this.currentState = data; console.log(this.currentState); },
      err => { },
      () => { }
    );
  }

  ngOnInit() {
  }

}
