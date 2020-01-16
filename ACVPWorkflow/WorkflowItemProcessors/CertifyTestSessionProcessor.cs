using System.Text.Json;
using ACVPCore;
using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CertifyTestSessionProcessor : IWorkflowItemProcessor
	{
		private readonly IValidationService _validationService;
		private readonly ITestSessionService _testSessionService;
		private readonly IWorkflowService _workflowService;

		public CertifyTestSessionProcessor(IValidationService validationService, ITestSessionService testSessionService, IWorkflowService workflowService)
		{
			_validationService = validationService;
			_testSessionService = testSessionService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			CertifyTestSessionParameters parameters = JsonSerializer.Deserialize<CertifyTestSessionParameters>(workflowItem.JSON);

			//This is the ugly stuff. Reference WorkflowProcessor.java's certifyTestSession for much of it. Here's the gist of what it does...

			//Do stuff with prereqs, not exactly sure what

			//Create an empty scenario

			//For each vector set that is passed (ignore cancelled ones, and anything else, though they shouldn't get to that point if there's anything else)
			//	Convert the registration to capabilities
			//	Add the prereqs for this algo
			//		If not self referential do a lookup - think this is only LCAVP relevant - based on source and ID

			//Put the OE on the scenario

			//Look for an existing validation for this product - logic here needs to change about what to update - ACVP should only update A validations. There's a chance that there are multiple if we've done goofy things to the data, so just take the latest - not going to try to figure out if it matches better with one of multiple validations. Should be an extremely rare case.
			long acvpValidationID = _validationService.GetLatestACVPValidationForImplementation(parameters.ImplementationID);

			if (acvpValidationID == 0)
			{
				//Create a new validation, all new stuff
			}
			else
			{
				//Update the existing validation - merge logic
			}

			//Create a "ValidationBuilder"
			//Add any existing scenarios if an existing validation
			//Do the merge logic
			//"Submit for validation"... not applicable because we're changing how it works

			//Update test session to published
			_testSessionService.UpdateStatus(parameters.TestSessionID, TestSessionStatus.Published);

			//Update the workflow item
			//if (validationResult.IsSuccess)
			//{
			//	_workflowService.MarkApproved(workflowItem.WorkflowItemID, validationResult.ID);
			//}
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
