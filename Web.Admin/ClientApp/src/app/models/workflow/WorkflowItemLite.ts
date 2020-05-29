import { APIAction } from './APIAction.enum';
import { WorkflowStatus } from './WorkflowStatus.enum';

export class WorkflowItemLite {
  workflowItemId: number;
  requestId: number;
  apiAction: APIAction;
  submissionId: string;
  submitter: string;
  submitted: Date;
  status: WorkflowStatus;

  // This is used for the multi-select on the Workflows page only
  multiSelected: boolean;
  multiSelectSubmissionStatus: string;
  multiApproveErrorReviewMessage: string;
}
