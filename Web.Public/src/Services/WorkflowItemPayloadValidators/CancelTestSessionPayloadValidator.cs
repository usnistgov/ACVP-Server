using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class CancelTestSessionPayloadValidator : IWorkflowItemValidator
	{
		private readonly ITestSessionService _testSessionService;

		public CancelTestSessionPayloadValidator(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var payload = (CancelPayload) workflowItemPayload;
			var errors = new List<string>();
			
			var testSession = _testSessionService.GetTestSession(payload.TestSessionID);
			if (testSession.Published)
			{
				errors.Add("testSession has already been published");
			}

			// Check that the test session exists is done by JWT claims
			return new PayloadValidationResult(errors);
		}
	}
}