import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../../models/workflow/IWorkflowItemPayload';
import { Result } from '../../../../models/responses/Result';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workflow-actions',
  templateUrl: './workflow-actions.component.html',
  styleUrls: ['./workflow-actions.component.scss']
})
export class WorkflowActionsComponent implements OnInit {

  workflowItem: WorkflowItemBase<IWorkflowItemPayload>;

  constructor(private workflowService: WorkflowProviderService, private router: Router) { }

  approveWorkflow() {
    this.workflowService.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => { this.refreshPageData(); },
      err => { },
      () => { }
    );
  }
  rejectWorkflow() {
    this.workflowService.rejectWorkflow(this.workflowItem.workflowItemID).subscribe(
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

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  @Input()
  set wfItem(workflowItem: WorkflowItemBase<IWorkflowItemPayload>) {
    console.log(this.workflowItem);
    this.workflowItem = workflowItem;
  }

  ngOnInit() {
  }

}
