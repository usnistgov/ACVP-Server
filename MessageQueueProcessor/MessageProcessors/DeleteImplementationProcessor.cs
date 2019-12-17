using System.Text.Json;
using ACVPWorkflow;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class DeleteImplementationProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public DeleteImplementationProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.DeleteImplementation, requestPayload.RequestID, requestPayload.Json.ToString(), requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.DeleteImplementation,
					JSON = requestPayload.Json.ToString()
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.DeleteImplementation);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
