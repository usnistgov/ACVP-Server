import { Component, OnInit, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/Workflow/WorkflowItemBase';
import { WorkflowDependencyCreatePayload } from '../../../../models/Workflow/Dependency/WorkflowDependencyCreatePayload';
import { AjaxService } from '../../../../services/ajax/ajax.service';
import { WorkflowStatus } from '../../../../models/Workflow/WorkflowStatus.enum';
import { WorkflowComponent } from '../../workflow/workflow.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-dependency-create',
  templateUrl: './workflow-dependency-create.component.html',
  styleUrls: ['./workflow-dependency-create.component.scss']
})
export class WorkflowDependencyCreateComponent implements OnInit {

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
