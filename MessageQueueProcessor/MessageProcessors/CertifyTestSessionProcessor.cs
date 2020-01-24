using System.Collections.Generic;
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
		private readonly IAlgorithmService _algorithmService;
		private readonly Dictionary<APIAction, bool> _autoApproveConfiguration;

		public CertifyTestSessionProcessor(ITestSessionService testSessionService, IValidationService validationService, IWorkflowService workflowService, IWorkflowItemProcessorFactory workflowItemProcessorFactory, IAlgorithmService algorithmService, Dictionary<APIAction, bool> autoApproveConfiguration)
		{
			_testSessionService = testSessionService;
			_validationService = validationService;
			_workflowService = workflowService;
			_workflowItemProcessorFactory = workflowItemProcessorFactory;
			_algorithmService = algorithmService;
			_autoApproveConfiguration = autoApproveConfiguration;
		}

		public void Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a CertifyTestSessionPayload object
			CertifyTestSessionPayload certifyTestSessionPayload = JsonSerializer.Deserialize<CertifyTestSessionPayload>(requestPayload.Json.ToString());

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

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CertifyTestSession, requestPayload.RequestID, requestPayload.Json.ToString(), requestPayload.UserID);

			//Update the test session to reflect that it has been submitted for approval
			_testSessionService.UpdateStatus(certifyTestSessionPayload.TestSessionID, TestSessionStatus.SubmittedForApproval);

			//Auto approve if configured to do so
			if (workflowInsertResult.IsSuccess && _autoApproveConfiguration.GetValueOrDefault(APIAction.CertifyTestSession))
			{
				//Build the workflow item to pass to the approval process
				WorkflowItem workflowItem = new WorkflowItem
				{
					WorkflowItemID = (long)workflowInsertResult.WorkflowID,
					APIAction = APIAction.CertifyTestSession,
					JSON = requestPayload.Json.ToString()
				};

				//Get the processor for this workflow item
				IWorkflowItemProcessor workflowItemProcessor = _workflowItemProcessorFactory.GetWorkflowItemProcessor(APIAction.CreateDependency);

				//Approve it
				workflowItemProcessor.Approve(workflowItem);
			}
		}
	}
}
