using System.Collections.Generic;
using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Services;
using NIST.CVP.Libraries.Internal.MessageQueue;
using NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class RequestProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;
		private readonly MessageQueueProcessorConfig _messageQueueProcessorConfig;

		public RequestProcessor(IWorkflowService workflowService, IWorkflowItemPayloadFactory workflowItemPayloadFactory, MessageQueueProcessorConfig messageQueueProcessorConfig)
		{
			_workflowService = workflowService;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_messageQueueProcessorConfig = messageQueueProcessorConfig;
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
			if (isValid && _messageQueueProcessorConfig.AutoApprove.GetValueOrDefault(apiAction))
			{
				//Approve it - don't care if this passes or fails, from the message processing standpoint the message has been fully processed
				_workflowService.Approve(workflowItem);
			}

			return new Result();
		}
	}
}
