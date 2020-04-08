import { PagedEnumerable } from '../responses/PagedEnumerable';

export class WorkflowListParameters extends PagedEnumerable {

  WorkflowItemId: string;
  APIActionID: string;
  RequestId: string;
  Status: string;

  public constructor(WorkflowItemId: string, APIActionID: string, RequestId: string, Status: string) {
    super();
    this.WorkflowItemId = WorkflowItemId;
    this.APIActionID = APIActionID;
    this.RequestId = RequestId;
    this.Status = Status;
  }
}
