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
	public class UpdateDependencyProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public UpdateDependencyProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a DependencyUpdatePayload object. This will automagically flag the fields that have been updated
			DependencyUpdatePayload dependencyUpdatePayload = JsonSerializer.Deserialize<DependencyUpdatePayload>(requestPayload.Json.ToString());

			//Create a DependencyUpdateParameters object to pass to the dependency service... Wonder if I can just cast it since it is so similar...
			DependencyUpdateParameters dependencyUpdateParameters = new DependencyUpdateParameters
			{
				ID = dependencyUpdatePayload.ID,
				Type = dependencyUpdatePayload.Type,
				Name = dependencyUpdatePayload.Name,
				Description = dependencyUpdatePayload.Description,
				Attributes = dependencyUpdatePayload.Attributes,
				TypeUpdated = dependencyUpdatePayload.TypeUpdated,
				NameUpdated = dependencyUpdatePayload.NameUpdated,
				DescriptionUpdated = dependencyUpdatePayload.DescriptionUpdated,
				AttributesUpdated = dependencyUpdatePayload.AttributesUpdated
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(dependencyUpdateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.UpdateDependency, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				 WorkflowItem workflowItem = new WorkflowItem
				 {
					 WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					 APIAction = APIAction.UpdateDependency,
					 JSON = json
				 };

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.UpdateDependency);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
