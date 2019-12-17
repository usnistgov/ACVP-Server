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
	public class CreateOEProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public CreateOEProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//TODO - Once rewrite Public, message.Payload will be the parameters object, so can remove much of this

			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a OECreatePayload object
			OECreatePayload oe = JsonSerializer.Deserialize<OECreatePayload>(requestPayload.Json.ToString());

			//Create a CreateOEParameters object to pass to the OE service
			OECreateParameters createOEParameters = new OECreateParameters
			{
				Name = oe.Name,
				DependencyURLs = oe.DependencyURLs
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(createOEParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CreateOE, requestPayload.RequestID, json, requestPayload.UserID);

			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CreateOE,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.CreateOE);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
