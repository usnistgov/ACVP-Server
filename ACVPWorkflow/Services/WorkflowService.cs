﻿using ACVPWorkflow.Providers;
using ACVPWorkflow.Results;

namespace ACVPWorkflow.Services
{
	public class WorkflowService : IWorkflowService
	{
		IWorkflowProvider _workflowProvider;
		IWorkflowContactProvider _workflowContactProvider;
		IRequestProvider _requestProvider;

		public WorkflowService(IWorkflowProvider workflowProvider, IWorkflowContactProvider workflowContactProvider, IRequestProvider requestProvider)
		{
			_workflowProvider = workflowProvider;
			_workflowContactProvider = workflowContactProvider;
			_requestProvider = requestProvider;
		}

		public Result MarkApproved(long workflowItemID, long objectID)
		{
			//Create the garabge payload we put on the workflow item that replaces anything useful
			//AcceptedWorkloadItemPayload payload = new AcceptedWorkloadItemPayload { URL = resultingObjectUrl };

			//Do the update of the workflow item
			return _workflowProvider.Update(workflowItemID, WorkflowStatus.Approved, objectID);
		}

		public Result UpdateStatus(long workflowItemID, WorkflowStatus workflowStatus)
		{
			return _workflowProvider.Update(workflowItemID, workflowStatus);
		}

		public WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, string payload, long userID)
		{
			//Get the contact info to put on the workflow item - TODO - kill this sometime, replaced by the userID going on the record after LCAVP dies
			WorkflowContact contact = BuildWorkflowContact(userID);

			//TODO - Kill this when redo public side and no longer need to carry the separate action and type on workflow items
			//Translate from the APIAction back to the legacy 2 part values
			(WorkflowItemType workflowItemType, RequestAction requestAction) = apiAction switch
			{
				APIAction.CreateDependency => (WorkflowItemType.Dependency, RequestAction.Create),
				APIAction.CreateImplementation => (WorkflowItemType.Implementation, RequestAction.Create),
				APIAction.CreateOE => (WorkflowItemType.OE, RequestAction.Create),
				APIAction.CreatePerson => (WorkflowItemType.Person, RequestAction.Create),
				APIAction.CreateVendor => (WorkflowItemType.Organization, RequestAction.Create),
				APIAction.UpdateDependency => (WorkflowItemType.Dependency, RequestAction.Update),
				APIAction.UpdateImplementation => (WorkflowItemType.Implementation, RequestAction.Update),
				APIAction.UpdateOE => (WorkflowItemType.OE, RequestAction.Update),
				APIAction.UpdatePerson => (WorkflowItemType.Person, RequestAction.Update),
				APIAction.UpdateVendor => (WorkflowItemType.Organization, RequestAction.Update),
				APIAction.DeleteDependency => (WorkflowItemType.Dependency, RequestAction.Delete),
				APIAction.DeleteImplementation => (WorkflowItemType.Implementation, RequestAction.Delete),
				APIAction.DeleteOE => (WorkflowItemType.OE, RequestAction.Delete),
				APIAction.DeletePerson => (WorkflowItemType.Person, RequestAction.Delete),
				APIAction.DeleteVendor => (WorkflowItemType.Organization, RequestAction.Delete),
				_ => (WorkflowItemType.Unknown, RequestAction.Create)
			};

			//Create the workflow item
			var result = _workflowProvider.Insert(apiAction, workflowItemType, requestAction, userID, payload, contact.Lab, contact.Name, contact.Email);

			//If it was successful, create the request record that goes with this item
			if (result.IsSuccess)
			{
				_requestProvider.Create(requestID, requestAction, (long)result.WorkflowID, userID);
			}

			return result;
		}

		private WorkflowContact BuildWorkflowContact(long acvpUserID)
		{
			return _workflowContactProvider.GetContactForACVPUser(acvpUserID);
		}
	}
}