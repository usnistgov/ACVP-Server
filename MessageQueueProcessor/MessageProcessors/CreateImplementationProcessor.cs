using System.Linq;
using System.Text.Json;
using ACVPCore;
using ACVPCore.Models.Parameters;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CreateImplementationProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public CreateImplementationProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
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

			//Deserialize the JSON into a ImplementationCreatePayload object
			ImplementationCreatePayload implementation = JsonSerializer.Deserialize<ImplementationCreatePayload>(requestPayload.Json.ToString());

			//Build a ImplementationCreateParameters object to pass to the Implementation service
			ImplementationCreateParameters implementationCreateParameters = new ImplementationCreateParameters
			{
				Name = implementation.Name,
				Description = implementation.Description,
				Type = ParseImplementationType(implementation.Type),
				Version = implementation.Version,
				Website = implementation.Website,
				OrganizationID = ParseID(implementation.VendorURL),
				AddressID = ParseID(implementation.AddressURL),
				ContactIDs = implementation.ContactURLs.Select(x => ParseID(x)).ToList(),
				IsITAR = false      //TODO - Do something for ITARs. For now, assuming nothing is ITAR
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(implementationCreateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CreateImplementation, requestPayload.RequestID, json, requestPayload.UserID);

			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CreateImplementation,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.CreateImplementation);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}

		private long ParseID(string url)
		{
			return long.Parse(url.Split("/")[^1]);
		}

		private ImplementationType ParseImplementationType(string type)
		{
			return type.ToLower() switch
			{
				"software" => ImplementationType.Software,
				"hardware" => ImplementationType.Hardware,
				"firmware" => ImplementationType.Firmware,
				_ => ImplementationType.Unknown
			};
		}
	}
}
