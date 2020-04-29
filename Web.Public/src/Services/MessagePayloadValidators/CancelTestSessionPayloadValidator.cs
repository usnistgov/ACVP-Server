using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class CancelTestSessionPayloadValidator : IMessagePayloadValidator
	{
		private readonly ITestSessionService _testSessionService;

		public CancelTestSessionPayloadValidator(ITestSessionService testSessionService)
		{
			_testSessionService = testSessionService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
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