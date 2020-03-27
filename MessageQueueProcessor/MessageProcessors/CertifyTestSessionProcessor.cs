using System.Collections.Generic;
using System.Text.Json;
using ACVPCore;
using ACVPCore.Services;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using NIST.CVP.MessageQueue;
using NIST.CVP.MessageQueue.MessagePayloads;
using NIST.CVP.Results;

namespace MessageQueueProcessor.MessageProcessors
{
	public class CertifyTestSessionProcessor : IMessageProcessor
	{
		private readonly ITestSessionService _testSessionService;
		private readonly IWorkflowService _workflowService;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;
		private readonly Dictionary<APIAction, bool> _autoApproveConfiguration;

		public CertifyTestSessionProcessor(ITestSessionService testSessionService, IWorkflowService workflowService, IWorkflowItemPayloadFactory workflowItemPayloadFactory, Dictionary<APIAction, bool> autoApproveConfiguration)
		{
			_testSessionService = testSessionService;
			_workflowService = workflowService;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
			_autoApproveConfiguration = autoApproveConfiguration;
		}

		public Result Process(Message message)
		{
			//Get the payload so we can get the Json
			RequestPayload requestPayload = JsonSerializer.Deserialize<RequestPayload>(message.Payload);

			//Deserialize the JSON into a CertifyTestSessionPayload object
			CertifyTestSessionPayload certifyTestSessionPayload = JsonSerializer.Deserialize<CertifyTestSessionPayload>(requestPayload.Json.ToString());

			//Create the workflow item
			WorkflowInsertResult workflowInsertResult = _workflowService.AddWorkflowItem(APIAction.CertifyTestSession, requestPayload.RequestID, requestPayload.Json.GetRawText(), requestPayload.UserID);

			//Error if that failed
			if (!workflowInsertResult.IsSuccess)
			{
				return workflowInsertResult;
			}

			//Any errors that occur beyond this point are not considered errors in the message processing, so will return message processing success even if the the workflow item is invalid or auto-approve fails

			//Build the workflow item to use in subsequent steps
			WorkflowItem workflowItem = new WorkflowItem
			{
				WorkflowItemID = (long)workflowInsertResult.WorkflowID,
				APIAction = APIAction.CertifyTestSession,
				Payload = _workflowItemPayloadFactory.GetPayload(requestPayload.Json.GetRawText(), APIAction.CertifyTestSession)
			};

			//Check that the test session status is appropriate for the certify step. You'd think this should somehow return an error the the user if not, but not seeing a way to do so...
			TestSessionStatus testSessionStatus = _testSessionService.GetStatus(certifyTestSessionPayload.TestSessionID);

			//Good to go if passed, otherwise have a variety of reasons to error
			if (testSessionStatus != TestSessionStatus.Passed)
			{
				string errorWithNoPlaceToGo = testSessionStatus switch
				{
					TestSessionStatus.Cancelled => "Test session has been cancelled",
					TestSessionStatus.Failed => "Test session contains failed vector sets",
					TestSessionStatus.PendingEvaluation => "Not all vector sets in test session have been evaluated",
					TestSessionStatus.Published => "Test session has already been approved",
					TestSessionStatus.SubmittedForApproval => "Test session has already been submitted for approval",
					_ => "Unknown test session status"
				};

				//Workflow item we just created needs to be rejected
				_workflowService.Reject(workflowItem);

				return new Result();
			}

			//Update the test session to reflect that it has been submitted for approval
			_testSessionService.UpdateStatus(certifyTestSessionPayload.TestSessionID, TestSessionStatus.SubmittedForApproval);
					   
			//Validate the workflow item - might immediately reject it
			bool isValid = _workflowService.Validate(workflowItem).IsSuccess;

			//If the workflow item was invalid we want to reset the test session status back to passed so they can retry. Yes, it seems silly that we just set it a few lines above and reset it here, but the payload validation is looking at the status
			if (!isValid)
			{
				_testSessionService.UpdateStatus(certifyTestSessionPayload.TestSessionID, TestSessionStatus.Passed);
			}

			//Auto approve if configured to do so
			if (isValid && _autoApproveConfiguration.GetValueOrDefault(APIAction.CertifyTestSession))
			{
				//Approve it - don't care if this passes or fails, from the message processing standpoint the message has been fully processed
				_workflowService.Approve(workflowItem);
			}

			return new Result();
		}
	}
}
