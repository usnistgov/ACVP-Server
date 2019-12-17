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
	public class UpdateImplementationProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public UpdateImplementationProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a ImplementationUpdatePayload object
			ImplementationUpdatePayload implementation = JsonSerializer.Deserialize<ImplementationUpdatePayload>(requestPayload.Json.ToString());

			//Build a ImplementationUpdateParameters object to pass to the Implementation service.
			ImplementationUpdateParameters implementationUpdateParameters = new ImplementationUpdateParameters
			{
				ID = implementation.ID,
				Name = implementation.Name,
				Description = implementation.Description,
				Type = ParseImplementationType(implementation.Type),
				Version = implementation.Version,
				Website = implementation.Website,
				OrganizationID = ParseOptionalID(implementation.VendorURL),
				AddressID = ParseOptionalID(implementation.AddressURL),
				ContactIDs = implementation.ContactURLs?.Select(x => ParseID(x))?.ToList(),
				NameUpdated = implementation.NameUpdated,
				DescriptionUpdated = implementation.DescriptionUpdated,
				TypeUpdated = implementation.TypeUpdated,
				VersionUpdated = implementation.VersionUpdated,
				WebsiteUpdated = implementation.WebsiteUpdated,
				OrganizationIDUpdated = implementation.VendorURLUpdated,
				AddressIDUpdated = implementation.AddressURLUpdated,
				ContactIDsUpdated = implementation.ContactURLsUpdated
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(implementationUpdateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.UpdateImplementation, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.UpdateImplementation,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.UpdateImplementation);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}

		private long? ParseOptionalID(string url)
		{
			string id = url?.Split("/")[^1];

			return string.IsNullOrEmpty(id) ? (long?)null : long.Parse(id);
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
