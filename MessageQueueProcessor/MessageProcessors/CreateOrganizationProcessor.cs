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
	public class CreateOrganizationProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public CreateOrganizationProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
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

			//Deserialize the JSON into a OrganizationCreatePayload object
			OrganizationCreatePayload organization = JsonSerializer.Deserialize<OrganizationCreatePayload>(requestPayload.Json.ToString());

			//Build a OrganizationCreateParameters object to pass to the Organization service
			OrganizationCreateParameters organizationCreateParameters = new OrganizationCreateParameters
			{
				Name = organization.Name,
				Website = organization.Website,
				VoiceNumber = organization.PhoneNumbers.FirstOrDefault(x => x.Type == "voice")?.Number,         //Though phone numbers are a collection of objects in the JSON, in the database there are just 2 fields on the org record
				FaxNumber = organization.PhoneNumbers.FirstOrDefault(x => x.Type == "fax")?.Number,
				ParentOrganizationID = ParseParentOrganizationID(organization.ParentURL),
				EmailAddresses = organization.EmailAddresses,
				Addresses = organization.Addresses.Select(x => new AddressCreateParameters
				{
					Street1 = x.Street1,
					Street2 = x.Street2,
					Street3 = x.Street3,
					Locality = x.Locality,
					Region = x.Region,
					PostalCode = x.PostalCode,
					Country = x.Country
				}).ToList()
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(organizationCreateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CreateVendor, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CreateVendor,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.CreateVendor);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}

		private long? ParseParentOrganizationID(string parentURL)
		{
			string parentID = parentURL?.Split("/")[^1];

			return string.IsNullOrEmpty(parentID) ? (long?)null : long.Parse(parentID);
		}
	}
}
