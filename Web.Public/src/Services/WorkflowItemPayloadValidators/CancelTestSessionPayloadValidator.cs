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

			var testSession = _testSessionService.GetTestSession(payload.TestSessionID);
			// TODO need published status on testSession here

			// Check that the test session exists is done by JWT claims
			return new PayloadValidationResult(new List<string>());
		}
	}
}