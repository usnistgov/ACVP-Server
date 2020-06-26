using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class CertifyTestSessionPayloadValidator : IMessagePayloadValidator
	{
		private readonly ITestSessionService _testSessionService;

		public CertifyTestSessionPayloadValidator(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var payload = (CertifyTestSessionPayload) workflowItemPayload;
			var errors = new List<string>();

			// Test session request must be from session owner
			// Done by claims in controller
            
			var testSession = _testSessionService.GetTestSession(payload.TestSessionID);
            
            if (testSession == null)
            {
            	errors.Add("Test session was not found");
            	return new PayloadValidationResult(errors);
            }
            
			// Test session must not be sample
			if (testSession.IsSample)
			{
				errors.Add("Sample test sessions may not be certified");
			}
            
			// Test session status must be passed
			if (testSession.Status != TestSessionStatus.Passed)
			{
				errors.Add("Test session must be in a passed state to be certified");
			}
            
			// TODO prerequisites

			return new PayloadValidationResult(errors);
		}
	}
}