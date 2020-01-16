using System.Linq;
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
	public class CreateDependencyProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public CreateDependencyProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
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

			//Deserialize the JSON into a DependencyCreatePayload object
			DependencyCreatePayload dependency = JsonSerializer.Deserialize<DependencyCreatePayload>(requestPayload.Json.ToString());

			//Create a CreateDependencyParameters object to pass to the dependency service
			DependencyCreateParameters createDependencyParameters = new DependencyCreateParameters
			{
				Type = dependency.Type,
				Name = dependency.Name,
				Description = dependency.Description,
				Attributes = dependency.Attributes.Select(a => new DependencyAttributeCreateParameters { Name = a.Key, Value = a.Value.GetString() }).ToList()
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(createDependencyParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CreateDependency, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CreateDependency,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.CreateDependency);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
