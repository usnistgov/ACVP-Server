import { APIAction } from './APIAction.enum';
import { IWorkflowItemPayload } from './IWorkflowItemPayload';
import { WorkflowStatus } from './WorkflowStatus.enum';

export class WorkflowItemLite {
  workflowItemID: number;
  apiAction: APIAction;
  status: WorkflowStatus;
}
