import { Injectable } from '@angular/core';
import { WorkflowItemBase } from '../../../models/Workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../models/Workflow/IWorkflowItemPayload';
import { WorkflowItemList } from '../../../models/Workflow/WorkflowItemList';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class WorkflowProviderService {

  apiRoot = "/api";

  constructor(private http: HttpClient) { }

  // BEGIN Workflow calls
  getWorkflow(workflowId: number) {
    return this.http.get<WorkflowItemBase<IWorkflowItemPayload>>(this.apiRoot + '/Workflows/' + workflowId);
  }

  getWorkflows(pageSize: number, pageNumber: number, RequestId: string, APIAction: string, DBID: string) {

    if (RequestId === "") { RequestId = null; }
    if (DBID === "") { DBID = null; }

    // Build the request body
    var params = {
      "pageSize": pageSize,
      "page": pageNumber,
      "requestId": RequestId,
      "APIActionID": APIAction,
      "WorkflowItemID": parseInt(DBID)
    };

    return this.http.post<WorkflowItemList>(this.apiRoot + '/Workflows', params);
  }

  approveWorkflow(workflowId: number) {
    return this.http.post(this.apiRoot + '/Workflows/' + workflowId + '/approve', {});
  }

  rejectWorkflow(workflowId: number) {
    return this.http.post(this.apiRoot + '/Workflows/' + workflowId + '/reject', {});
  }
  // End Workflow Calls
}
