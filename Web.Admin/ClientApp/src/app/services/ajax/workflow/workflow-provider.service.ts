import { Injectable } from '@angular/core';
import { WorkflowItemBase } from '../../../models/Workflow/WorkflowItemBase';
import { IWorkflowItemPayload } from '../../../models/Workflow/IWorkflowItemPayload';
import { WorkflowItemList } from '../../../models/Workflow/WorkflowItemList';
import { HttpClient } from '@angular/common/http';
import { WorkflowListParameters } from '../../../models/Workflow/WorkflowListParameters';

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

  getWorkflows(params: WorkflowListParameters) {

    if (params.RequestId === "") { params.RequestId = null; }
    if (params.APIActionID === "") { params.APIActionID = null; }
    if (params.WorkflowItemId === "") { params.WorkflowItemId = null; }
    if (params.Status === "") { params.Status = null; }

    // Build the request body
    var slightlyReformatted = {
      "pageSize": params.pageSize,
      "page": params.page,
      "RequestId": params.RequestId,
      "APIActionID": params.APIActionID,
      "WorkflowItemID": parseInt(params.WorkflowItemId),
      "Status": params.Status
    };

    return this.http.post<WorkflowItemList>(this.apiRoot + '/Workflows', slightlyReformatted);
  }

  approveWorkflow(workflowId: number) {
    return this.http.post(this.apiRoot + '/Workflows/' + workflowId + '/approve', {});
  }

  rejectWorkflow(workflowId: number) {
    return this.http.post(this.apiRoot + '/Workflows/' + workflowId + '/reject', {});
  }
  // End Workflow Calls
}
