using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class UpdateOEProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public UpdateOEProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a OEUpdatePayload object. This will automagically flag the fields that have been updated
			OEUpdatePayload oeUpdatePayload = JsonSerializer.Deserialize<OEUpdatePayload>(requestPayload.Json.ToString());

			OEUpdateParameters oeUpdateParameters = new OEUpdateParameters
			{
				ID = oeUpdatePayload.ID,
				Name = oeUpdatePayload.Name,
				DependencyURLs = oeUpdatePayload.DependencyURLs,
				NameUpdated = oeUpdatePayload.NameUpdated,
				DependenciesUpdated = oeUpdatePayload.DependenciesUpdated
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(oeUpdateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.UpdateOE, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.UpdateOE,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.UpdateOE);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
