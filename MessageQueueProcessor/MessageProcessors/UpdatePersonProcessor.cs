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
	public class UpdatePersonProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public UpdatePersonProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a PersonUpdatePayload object
			PersonUpdatePayload person = JsonSerializer.Deserialize<PersonUpdatePayload>(requestPayload.Json.ToString());

			//Build a PersonUpdateParameters object to pass to the Person service.
			PersonUpdateParameters personUpdateParameters = new PersonUpdateParameters
			{
				ID = person.ID,
				Name = person.Name,
				OrganizationID = ParseOrganizationID(person.VendorURL),
				PhoneNumbers = person.PhoneNumbers?.Select(x => (x.Type, x.Number))?.ToList(),
				EmailAddresses = person.EmailAddresses,
				NameUpdated = person.NameUpdated,
				OrganizationIDUpdated = person.VendorURLUpdated,
				PhoneNumbersUpdated = person.PhoneNumbersUpdated,
				EmailAddressesUpdated = person.EmailAddressesUpdated
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(personUpdateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.UpdatePerson, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.UpdatePerson,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.UpdatePerson);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}

		private long? ParseOrganizationID(string organizationUrl)
		{
			string parentID = organizationUrl?.Split("/")[^1];

			return string.IsNullOrEmpty(parentID) ? (long?)null : long.Parse(parentID);
		}
	}
}
