using System.Text.Json;
using ACVPCore;
using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using ACVPWorkflow.WorkflowItemProcessors;
using MessageQueueProcessor.MessagePayloads;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CertifyTestSessionProcessor : IMessageProcessor
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IValidationService _validationService;
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemProcessorFactory _workflowItemProcessorFactory;
		private readonly bool _autoApprove;

		public CertifyTestSessionProcessor(ITestSessionService testSessionService, IValidationService validationService, IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, bool autoApprove)
		{
			_testSessionService = testSessionService;
			_validationService = validationService;
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_autoApprove = autoApprove;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a CertifyTestSessionPayload object. Unlike all other types, we don't touch the JSON here, this is just a passthrough
			CertifyTestSessionPayload certifyTestSessionPayload = JsonSerializer.Deserialize<CertifyTestSessionPayload>(requestPayload.Json.ToString());

			CertifyTestSessionParameters certifyTestSessionParameters = new CertifyTestSessionParameters
			{
				TestSessionID = certifyTestSessionPayload.TestSessionID,
				ImplementationID = long.Parse(certifyTestSessionPayload.ImplementationURL.Split("/")[^1]),
				OEID = long.Parse(certifyTestSessionPayload.OEURL.Split("/")[^1])
			};

			//Check that the test session status is appropriate for the certify step. You'd think this should somehow return an error the the user if not, but not seeing a way to do so...
			//Get the current status
			TestSessionStatus testSessionStatus = _testSessionService.GetStatus(certifyTestSessionPayload.TestSessionID);

			//Good to go if passed, otherwise have a variety of reasons to error
			if (testSessionStatus != TestSessionStatus.Passed)
			{
				string errorMessageWithNoPlaceToGo = testSessionStatus switch
				{
					TestSessionStatus.Cancelled => "Test session has been cancelled",
					TestSessionStatus.Failed => "Test session contains failed vector sets",
					TestSessionStatus.PendingEvaluation => "Not all vector sets in test session have been evaluated",
					TestSessionStatus.Published => "Test session has already been approved",
					TestSessionStatus.SubmittedForApproval => "Test session has already been submitted for approval",
					_ => "Unknown test session status"
				};

				return;
			}

			//Serialize the parameters back to JSON to go on the workflow item
			string json = JsonSerializer.Serialize(certifyTestSessionParameters);

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CertifyTestSession, requestPayload.RequestID, json, requestPayload.UserID);

			//Update the test session to reflect that it has been submitted for approval
			_testSessionService.UpdateStatus(certifyTestSessionPayload.TestSessionID, TestSessionStatus.SubmittedForApproval);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApprove)
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CertifyTestSession,
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
