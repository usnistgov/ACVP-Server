import { Component, OnInit, Input } from '@angular/core';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { Router } from '@angular/router';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';

@Component({
  selector: 'app-workflow-dependency-delete',
  templateUrl: './workflow-dependency-delete.component.html',
  styleUrls: ['./workflow-dependency-delete.component.scss']
})
export class WorkflowDependencyDeleteComponent implements OnInit {

  constructor(private ajs: AjaxService, private router: Router) { }

  //approveWorkflow() {
  //  this.ajs.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
  //    data => { this.refreshPageData(); },
  //    err => { },
  //    () => { }
  //  );
  //}
  //rejectWorkflow() {
  //  this.ajs.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
  //    data => { this.refreshPageData(); },
  //    err => { },
  //    () => { }
  //  );
  //}

  //refreshPageData() {
  //  this.router.navigateByUrl('/', { skipLocationChange: true })
  //    .then(() =>
  //      this.router.navigate(['workflow/' + this.workflowItem.workflowItemID])
  //    );
  //}

  //workflowItem: WorkflowItemBase<WorkflowDependencyDeletePayload>;
  objectKeys = Object.keys;

  /*
   * This is how the component takes the workflowItem from the main workflow controller using the
   * [wfItem]="workflowItem" syntax in the HTML template.  i.e. how custom element attributes are specified
   */
  //@Input()
  //set wfItem(item: WorkflowItemBase<WorkflowDependencyDeletePayload>) {
  //  this.workflowItem = item;
  //}

  ngOnInit() {
  }

}
