using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class CertifyTestSessionPayloadValidator : IWorkflowItemValidator
	{
		private readonly ITestSessionService _testSessionService;

		public CertifyTestSessionPayloadValidator(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var payload = (CertifyTestSessionPayload) workflowItemPayload;
			var errors = new List<string>();

			// Test session request must be from session owner
			if (!_testSessionService.IsOwner(payload.UserCertificate, payload.TestSessionID))
			{
				errors.Add("Certify request must be submitted by the test session owner");
			}
            
			var testSession = _testSessionService.GetTestSession(payload.TestSessionID);
            
			// Test session must not be sample
			if (testSession.IsSample)
			{
				errors.Add("Sample test sessions may not be certified");
			}

			// TODO what makes a test session not publishable?
			if (!testSession.Publishable)
			{
				errors.Add("Test session not publishable");
			}
            
			// Test session must be passing
			if (!testSession.Passed)
			{
				errors.Add("Test session must be in a passed state to be certified");
			}
            
			// TODO prerequisites

			return new PayloadValidationResult(errors);
		}
	}
}