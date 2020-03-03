import { APIAction } from './APIAction.enum';
import { IWorkflowItemPayload } from './IWorkflowItemPayload';
import { WorkflowStatus } from './WorkflowStatus.enum';

export class WorkflowItemBase {
  WorkflowItemId: number;
  APIAction: APIAction;
  payload: IWorkflowItemPayload;
  status: WorkflowStatus;
}
