using System;
using System.Collections.Generic;
using ACVPCore.ExtensionMethods;
using ACVPCore.Results;
using ACVPCore.Models;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
using ACVPWorkflow.Providers;
using ACVPWorkflow.Results;
using Microsoft.Extensions.Logging;

namespace ACVPWorkflow.Services
{
	public class WorkflowService : IWorkflowService
	{
		private readonly ILogger<WorkflowService> _logger;
		private readonly IWorkflowProvider _workflowProvider;
		private readonly IWorkflowContactProvider _workflowContactProvider;
		private readonly IRequestProvider _requestProvider;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		

		public WorkflowService(
			ILogger<WorkflowService> logger,
			IWorkflowProvider workflowProvider, 
			IWorkflowContactProvider workflowContactProvider, 
			IRequestProvider requestProvider, 
			IWorkflowItemPayloadFactory workflowItemPayloadFactory,
			IWorkflowItemProcessorFactory workflowItemProcessorFactory)
		{
			_logger = logger;
			_workflowProvider = workflowProvider;
			_workflowContactProvider = workflowContactProvider;
			_requestProvider = requestProvider;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
		}

		public Result Validate(WorkflowItem workflowItem)
		{
			//Get the right processor for this kind of workflow item
			var workflowProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(workflowItem.APIAction);

			try
			{
				bool isValid = workflowProcessor.Validate(workflowItem);

				//If it failed it would throw an exception, not return false...
				return new Result();
			}
			catch (Exception ex) when (ex is ResourceInUseException 
									|| ex is ResourceDoesNotExistException
									|| ex is BusinessRuleException)
			{
				UpdateStatus(workflowItem, WorkflowStatus.Rejected);
				_logger.LogWarning(ex);
				return new Result(ex.Message);
			}
			catch (NotPendingApprovalException ex)
			{
				//No change to the workflow item status because we're simply preventing it from being acted upon again
				_logger.LogWarning(ex);
				return new Result(ex.Message);
			}
		}


		public Result Approve(WorkflowItem workflowItem)
		{
			//Get the right processor for this kind of workflow item
			var workflowProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(workflowItem.APIAction);

			try
			{
				var result = workflowProcessor.Approve(workflowItem);
				
				//Do the update of the workflow item
				return _workflowProvider.Update(workflowItem.WorkflowItemID, WorkflowStatus.Approved, result);
			}
			catch (Exception ex) when (ex is ResourceInUseException 
									|| ex is ResourceDoesNotExistException
									|| ex is BusinessRuleException)
			{
				UpdateStatus(workflowItem, WorkflowStatus.Rejected);
				_logger.LogWarning(ex);
				return new Result(ex.Message);
			}
			catch (NotPendingApprovalException ex)
			{
				//No change to the workflow item status because we're simply preventing it from being acted upon again
				_logger.LogWarning(ex);
				return new Result(ex.Message);
			}
			catch (ResourceProcessorException ex)
			{
				UpdateStatus(workflowItem, WorkflowStatus.Incomplete);
				_logger.LogError(ex);
				return new Result(ex.Message);
			}
		}

		public Result Reject(WorkflowItem workflowItem)
		{
			var workflowProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(workflowItem.APIAction);
			workflowProcessor.Reject(workflowItem);
			return UpdateStatus(workflowItem, WorkflowStatus.Rejected);
		}

		private Result UpdateStatus(WorkflowItem workflowItem, WorkflowStatus workflowStatus)
		{
			return _workflowProvider.Update(workflowItem.WorkflowItemID, workflowStatus);
		}
		
		public WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, IWorkflowItemPayload payload, long userID)
		{
			//Get the contact info to put on the workflow item - TODO - kill this sometime, replaced by the userID going on the record after LCAVP dies
			WorkflowContact contact = BuildWorkflowContact(userID);

			//TODO - Kill this when redo public side and no longer need to carry the separate action and type on workflow items
			//Translate from the APIAction back to the legacy 2 part values
			(WorkflowItemType workflowItemType, RequestAction requestAction) = apiAction switch
			{
				APIAction.CertifyTestSession => (WorkflowItemType.Validation, RequestAction.Create),
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
			string stringPayload = _workflowItemPayloadFactory.SerializePayload(payload);
			var result = _workflowProvider.Insert(apiAction, workflowItemType, requestAction, userID, stringPayload, contact.Lab, contact.Name, contact.Email);

			//If it was successful, create the request record that goes with this item
			if (result.IsSuccess)
			{
				_requestProvider.Create(requestID, requestAction, (long)result.WorkflowID, userID);
			}

			return result;
		}

		public PagedEnumerable<WorkflowItemLite> GetWorkflowItems(WorkflowListParameters param)
		{
			return _workflowProvider.GetWorkflowItems(param);
		}

		public WorkflowItem GetWorkflowItem(long workflowItemId)
		{
			return _workflowProvider.GetWorkflowItem(workflowItemId);
		}

		private WorkflowContact BuildWorkflowContact(long acvpUserID)
		{
			return _workflowContactProvider.GetContactForACVPUser(acvpUserID);
		}
	}
}
