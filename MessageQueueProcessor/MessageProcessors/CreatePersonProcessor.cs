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
	public class CreatePersonProcessor : IMessageProcessor
	{
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public CreatePersonProcessor(IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
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

			//Deserialize the JSON into a PersonCreatePayload object
			PersonCreatePayload person = JsonSerializer.Deserialize<PersonCreatePayload>(requestPayload.Json.ToString());

			//Build a PersonCreateParameters object to pass to the Person service
			PersonCreateParameters personCreateParameters = new PersonCreateParameters
			{
				Name = person.Name,
				OrganizationID = long.Parse(person.VendorURL.Split("/")[^1]),
				PhoneNumbers = person.PhoneNumbers.Select(x => (x.Type, x.Number)).ToList(),
				EmailAddresses = person.EmailAddresses,
			};

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(personCreateParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CreatePerson, requestPayload.RequestID, json, requestPayload.UserID);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CreatePerson,
					JSON = json
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.CreatePerson);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
