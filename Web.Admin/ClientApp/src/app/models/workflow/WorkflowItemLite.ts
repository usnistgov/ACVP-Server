import { APIAction } from './APIAction.enum';
import { WorkflowStatus } from './WorkflowStatus.enum';

export class WorkflowItemLite {
  workflowItemID: number;
  requestId: number;
  apiAction: APIAction;
  submissionId: string;
  submitter: string;
  submitted: Date;
  status: WorkflowStatus;
}
