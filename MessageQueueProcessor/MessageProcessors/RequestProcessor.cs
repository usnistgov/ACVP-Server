using System.Collections.Generic;
using System.Text.Json;
using ACVPWorkflow;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class RequestProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly Dictionary<APIAction, bool> _autoApproveConfiguration;

		public RequestProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, Dictionary<APIAction, bool> autoApproveConfiguration)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApproveConfiguration = autoApproveConfiguration;
		}

		public void Process(Message message)
		{
			//Grab the action since it takes a little thinking to get it, and may use multiple times. In the future the message will contain this natively
			APIAction apiAction = message.Action;

			//Deserialize the payload
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(apiAction, requestPayload.RequestID, requestPayload.Json.ToString(), requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApproveConfiguration.GetValueOrDefault(apiAction))
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = apiAction,
					JSON = requestPayload.Json.ToString()
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(apiAction);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
