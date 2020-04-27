import { APIAction } from './APIAction.enum';
import { WorkflowStatus } from './WorkflowStatus.enum';

export class WorkflowItemBase<PayloadType> {
  workflowItemID: number;
  requestId: number;
  apiAction: APIAction;
  payload: PayloadType;
  status: WorkflowStatus;
}
