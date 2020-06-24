import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { WorkflowItemLite } from '../../../models/workflow/WorkflowItemLite';
import { Result } from '../../../models/responses/Result';
import { WorkflowProviderService } from '../../../services/ajax/workflow/workflow-provider.service';

@Component({
  selector: 'app-workflows-multi-approve',
  templateUrl: './workflows-multi-approve.component.html',
  styleUrls: ['./workflows-multi-approve.component.scss']
})
export class WorkflowsMultiApproveComponent implements OnInit {

  requestsToApprove: WorkflowItemLite[];

  multiApproveModalTitle = "Are you sure that you want to approve all these items?";
  multiApproveErrorReviewMessage = "";

  constructor(private workflowService: WorkflowProviderService) { }

  @Input()
  set requests(requestsToApprove: WorkflowItemLite[]) {
    this.requestsToApprove = requestsToApprove;
  }

  // https://www.themarketingtechnologist.co/building-nested-components-in-angular-2/
  @Output() notifyParentComponent: EventEmitter<Result> = new EventEmitter<Result>();

  isApprovalComplete(element: WorkflowItemLite, index, array) {
    if (element.multiSelectSubmissionStatus === "successful" ||
      element.multiSelectSubmissionStatus === "error") { return true; }
    return false;
  }

  closeMultiApproveModal() {
    this.notifyParentComponent.emit();
    this.multiApproveModalTitle = "Are you sure that you want to approve all these items?";
  }

  submitApprovals() {
    this.multiApproveModalTitle = "Approving selected items...";

    this.requestsToApprove.forEach(function (item, index, array) {
      item.multiSelectSubmissionStatus = "waiting..."
    });

    this.approveWorkflowRecursive(0);
  }

  approveWorkflowRecursive(index: number) {
    var self = this;

    // Base case - if we've reached the end
    if (index == this.requestsToApprove.length) {
      return null;
    }

    // Recursive case
    else {
      self.workflowService.approveWorkflow(this.requestsToApprove[index].workflowItemId)
        .subscribe(data => {

          // Store the relevant values, like error message and completion status
          if (data.isSuccess) {
            this.requestsToApprove[index].multiSelectSubmissionStatus = "successful";
          }
          else {
            this.requestsToApprove[index].multiSelectSubmissionStatus = "error";
            this.requestsToApprove[index].multiApproveErrorReviewMessage = data.errorMessage;
          }

          if (this.requestsToApprove.filter(self.isApprovalComplete).length > 0) {
            self.multiApproveModalTitle = "All Approvals Complete";
          }

          // Then, call this function again, incrementing the index
          self.approveWorkflowRecursive(index + 1);
        });
    }
  }

  ngOnInit() {
  }

}
