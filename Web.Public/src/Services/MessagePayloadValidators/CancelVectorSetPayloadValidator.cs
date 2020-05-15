using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class CancelVectorSetPayloadValidator : IMessagePayloadValidator
	{
		private readonly ITestSessionService _testSessionService;

		public CancelVectorSetPayloadValidator(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var payload = (CancelPayload) workflowItemPayload;
			var errors = new List<string>();
			
			// Check testSession has not been published yet
			var testSession = _testSessionService.GetTestSession(payload.TestSessionID);
			if (testSession.Status == TestSessionStatus.Published)
			{
				errors.Add("testSession parent of vectorSet has already been published");
			}
			
			// Check vector set is not the only element of the testSession
			if (testSession.VectorSetIDs.Count == 1)
			{
				errors.Add("vectorSet is the only set within the testSession, cannot cancel");
			}
			
			// Check that the vector set exists and is an element of the test session is done by JWT claims
			return new PayloadValidationResult(errors);
		}
	}
}