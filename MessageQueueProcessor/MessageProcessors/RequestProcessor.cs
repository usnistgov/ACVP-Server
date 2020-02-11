using System.Collections.Generic;
using System.Text.Json;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class RequestProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;
		private readonly Dictionary<APIAction, bool> _autoApproveConfiguration;

		public RequestProcessor(IWorkflowService workflowService, IWorkflowItemPayloadFactory workflowItemPayloadFactory, Dictionary<APIAction, bool> autoApproveConfiguration)
		{
			_workflowService = workflowService;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_autoApproveConfiguration = autoApproveConfiguration;
		}

		public void Process(Message message)
		{
			//Grab the action since it takes a little thinking to get it, and may use multiple times. In the future the message will contain this natively
			APIAction apiAction = message.Action;

			//Deserialize the request payload
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);
			
			//The Json element in this is actually a workflow payload, so jump through hoops to deserialize it
			IWorkflowItemPayload workflowPayload = _workflowItemPayloadFactory.GetPayload(requestPayload.Json.GetRawText(), apiAction);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(apiAction, requestPayload.RequestID, workflowPayload, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApproveConfiguration.GetValueOrDefault(apiAction))
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = apiAction,
					Payload = workflowPayload
				};

				//Approve it
				_workflowService.Approve(workflowItem);
			}
		}
	}
}
