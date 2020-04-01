using System.Text.Json;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;
using Microsoft.Extensions.Configuration;
using NIST.CVP.MessageQueue;
using NIST.CVP.MessageQueue.MessagePayloads;
using NIST.CVP.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class RequestProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;
		private readonly IConfigurationSection _autoApproveConfiguration;

		public RequestProcessor(IWorkflowService workflowService, IWorkflowItemPayloadFactory workflowItemPayloadFactory, IConfigurationSection autoApproveConfiguration)
		{
			_workflowService = workflowService;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_autoApproveConfiguration = autoApproveConfiguration;
		}

		public Result Process(Message message)
		{
			//Grab the action since it takes a little thinking to get it, and may use multiple times. In the future the message will contain this natively
			APIAction apiAction = message.Action;

			//Deserialize the request payload
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Create the workflow item
			var workflowInsertResult = _workflowService.AddWorkflowItem(apiAction, requestPayload.RequestID, requestPayload.Json.GetRawText(), requestPayload.UserID);

			if (!workflowInsertResult.IsSuccess)
			{
				return workflowInsertResult;
			}

			//Any errors that occur beyond this point are not considered errors in the message processing, so will return message processing success even if the the workflow item is invalid or auto-approve fails

			//Build the workflow item to use in subsequent steps
			WorkflowItem workflowItem = new WorkflowItem
			{
				WorkflowItemID = (long)workflowInsertResult.WorkflowID,
				APIAction = apiAction,
				Payload = _workflowItemPayloadFactory.GetPayload(requestPayload.Json.GetRawText(), apiAction)
			};

			//Validate the workflow item - might immediately reject it
			bool isValid = _workflowService.Validate(workflowItem).IsSuccess;

			//Auto approve if configured to do so
			if (isValid && _autoApproveConfiguration.GetValue<bool>(apiAction.ToString()))
			{
				//Approve it - don't care if this passes or fails, from the message processing standpoint the message has been fully processed
				_workflowService.Approve(workflowItem);
			}

			return new Result();
		}
	}
}
