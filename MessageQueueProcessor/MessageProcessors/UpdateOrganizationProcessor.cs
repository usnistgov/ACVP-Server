using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class UpdateOrganizationProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public UpdateOrganizationProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a OrganizationUpdatePayload object
			OrganizationUpdatePayload organization = JsonSerializer.Deserialize<OrganizationUpdatePayload>(requestPayload.Json.ToString());

			//Build a OrganizationCreateParameters object to pass to the Organization service. The nested addresses are a bit goofy
			List<object> AddressObjects = new List<object>();

			for (int i = 0; i < organization.Addresses.Count; i++)
			{
				var addressParameters = organization.Addresses[i];

				if (string.IsNullOrEmpty(addressParameters.URL))
				{
					//This will be a new address
					AddressObjects.Add(new AddressCreateParameters
					{
						OrganizationID = organization.ID,
						OrderIndex = i,
						Street1 = addressParameters.Street1,
						Street2 = addressParameters.Street2,
						Street3 = addressParameters.Street3,
						Locality = addressParameters.Locality,
						Region = addressParameters.Region,
						PostalCode = addressParameters.PostalCode,
						Country = addressParameters.Country
					});
				}
				else
				{
					//This is an address update
					AddressObjects.Add(new AddressUpdateParameters
					{
						ID = long.Parse(addressParameters.URL.Split("/")[^1]),
						OrderIndex = i,
						Street1 = addressParameters.Street1,
						Street2 = addressParameters.Street2,
						Street3 = addressParameters.Street3,
						Locality = addressParameters.Locality,
						Region = addressParameters.Region,
						PostalCode = addressParameters.PostalCode,
						Country = addressParameters.Country,
						Street1Updated = addressParameters.Street1Updated,
						Street2Updated = addressParameters.Street2Updated,
						Street3Updated = addressParameters.Street3Updated,
						LocalityUpdated = addressParameters.LocalityUpdated,
						RegionUpdated = addressParameters.RegionUpdated,
						PostalCodeUpdated = addressParameters.PostalCodeUpdated,
						CountryUpdated = addressParameters.CountryUpdated
						//Not passing org id because can't update that
					});
				}
			}

			OrganizationUpdateParameters organizationUpdateParameters = new OrganizationUpdateParameters
			{
				ID = organization.ID,
				Name = organization.Name,
				Website = organization.Website,
				VoiceNumber = organization.PhoneNumbers.FirstOrDefault(x => x.Type == "voice")?.Number,         //Though phone numbers are a collection of objects in the JSON, in the database there are just 2 fields on the org record
				FaxNumber = organization.PhoneNumbers.FirstOrDefault(x => x.Type == "fax")?.Number,
				ParentOrganizationID = ParseParentOrganizationID(organization.ParentOrganizationURL),
				EmailAddresses = organization.EmailAddresses,
				Addresses = AddressObjects,
				NameUpdated = organization.NameUpdated,
				WebsiteUpdated = organization.WebsiteUpdated,
				ParentOrganizationIDUpdated = organization.ParentOrganizationURLUpdated,
				PhoneNumbersUpdated = organization.PhoneNumbersUpdated,
				EmailAddressesUpdated = organization.EmailAddressesUpdated,
				AddressesUpdated = organization.AddressesUpdated
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(organizationUpdateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.UpdateVendor, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.UpdateVendor,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.UpdateVendor);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}

		private long? ParseParentOrganizationID(string parentUrl)
		{
			string parentID = parentUrl?.Split("/")[^1];

			return string.IsNullOrEmpty(parentID) ? (long?)null : long.Parse(parentID);
		}
	}
}
