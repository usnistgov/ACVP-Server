using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class CertifyTestSessionPayloadValidator : IMessagePayloadValidator
	{
		private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
		private readonly ITestSessionService _testSessionService;
		private readonly IOEService _oeService;
		private readonly IImplementationService _implementationService;
		
		public CertifyTestSessionPayloadValidator(IMessagePayloadValidatorFactory workflowItemValidatorFactory, ITestSessionService testSessionService, IOEService oeService, IImplementationService implementationService)
		{
			_workflowItemValidatorFactory = workflowItemValidatorFactory;
			_testSessionService = testSessionService;
			_oeService = oeService;
			_implementationService = implementationService;
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

			if (string.IsNullOrEmpty(payload.OEURL) && payload.OEToCreate == null)
			{
				errors.Add("An OE URL or inline OE create must be present in the payload.");
			}
			
			if (string.IsNullOrEmpty(payload.ImplementationURL) && payload.ImplementationToCreate == null)
			{
				errors.Add("A module URL or inline module create must be present in the payload.");
			}
			
			if (!string.IsNullOrEmpty(payload.OEURL) && payload.OEToCreate != null)
			{
				errors.Add("Only submit one of an oeUrl or OE inline create, not both.");
			}
			
			if (!string.IsNullOrEmpty(payload.ImplementationURL) && payload.ImplementationToCreate != null)
			{
				errors.Add("Only submit one of a moduleUrl or module inline create, not both.");
			}
			
			if (!string.IsNullOrEmpty(payload.OEURL))
			{
				if (!_oeService.Exists(BasePayload.ParseIDFromURL(payload.OEURL)))
				{
					errors.Add($"Invalid oeUrl provided: {payload.OEURL}");
				}
			}
			
			if (!string.IsNullOrEmpty(payload.ImplementationURL))
			{
				if (!_implementationService.Exists(BasePayload.ParseIDFromURL(payload.ImplementationURL)))
				{
					errors.Add($"Invalid moduleUrl provided: {payload.ImplementationURL}");
				}
			}

			if (payload.ImplementationToCreate != null)
			{
				var inlineImplementationCreateValidation = _workflowItemValidatorFactory
					.GetMessagePayloadValidator(APIAction.CreateImplementation)
					.Validate(payload.ImplementationToCreate);
				
				errors.AddRangeIfNotNullOrEmpty(inlineImplementationCreateValidation.Errors);
			}
			
			if (payload.OEToCreate != null)
			{
				var inlineOeCreateValidation = _workflowItemValidatorFactory
					.GetMessagePayloadValidator(APIAction.CreateOE)
					.Validate(payload.OEToCreate);
				
				errors.AddRangeIfNotNullOrEmpty(inlineOeCreateValidation.Errors);
			}
			
			// TODO prerequisites

			return new PayloadValidationResult(errors);
		}
	}
}