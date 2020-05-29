import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { WorkflowItemBase } from '../../../../models/workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../../models/workflow/IWorkflowItemPayload';
import { Result } from '../../../../models/responses/Result';
import { WorkflowProviderService } from '../../../../services/ajax/workflow/workflow-provider.service';
import { Router } from '@angular/router';
import { ModalService } from '../../../../services/modal/modal.service';

@Component({
  selector: 'app-workflow-actions',
  templateUrl: './workflow-actions.component.html',
  styleUrls: ['./workflow-actions.component.scss']
})
export class WorkflowActionsComponent implements OnInit {

  errorMessage: string;
  approvalStatusFlag = "";

  workflowItem: WorkflowItemBase<IWorkflowItemPayload>;

  constructor(private workflowService: WorkflowProviderService, private router: Router, private ModalService: ModalService) { }

  approveWorkflow() {

    this.approvalStatusFlag = "processing";

    this.workflowService.approveWorkflow(this.workflowItem.workflowItemID).subscribe(
      data => {
        if (data.isSuccess) {
          this.approvalStatusFlag = "success";
          this.refreshPageData();
        }
        else {
          this.approvalStatusFlag = "";
          this.errorMessage = data.errorMessage;
          this.ModalService.showModal('WFApprovalErrorMOdal');
        }
      },
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
