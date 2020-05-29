using System;
using Microsoft.Extensions.Logging;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Services
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
			catch (Exception ex)
			{
				_logger.LogError(ex);
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
			catch (Exception ex)
			{
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
		
		public WorkflowInsertResult AddWorkflowItem(APIAction apiAction, long requestID, string payload, long userID)
		{
			//Get the contact info to put on the workflow item - TODO - kill this sometime, replaced by the userID going on the record after LCAVP dies
			WorkflowContact contact = BuildWorkflowContact(userID);

			//Create the workflow item
			var result = _workflowProvider.Insert(apiAction, userID, payload, contact.Lab, contact.Name, contact.Email);

			//If it was successful, create the request record that goes with this item
			if (result.IsSuccess)
			{
				_requestProvider.Create(requestID, (long)result.WorkflowID, userID);
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
