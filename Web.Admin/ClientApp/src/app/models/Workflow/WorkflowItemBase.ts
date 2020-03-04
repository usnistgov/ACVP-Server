import { APIAction } from './APIAction.enum';
import { IWorkflowItemPayload } from './IWorkflowItemPayload';
import { WorkflowStatus } from './WorkflowStatus.enum';

export class WorkflowItemBase<PayloadType> {
  workflowItemID: number;
  apiAction: APIAction;
  payload: PayloadType;
  status: WorkflowStatus;
}
