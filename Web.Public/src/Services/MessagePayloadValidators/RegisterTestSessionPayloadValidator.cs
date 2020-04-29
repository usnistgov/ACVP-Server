using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class RegisterTestSessionPayloadValidator : IMessagePayloadValidator
	{
		private readonly IParameterValidatorService _parameterValidatorService;

		public RegisterTestSessionPayloadValidator(IParameterValidatorService parameterValidatorService)
		{
			_parameterValidatorService = parameterValidatorService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var payload = (TestSessionRegisterPayload) workflowItemPayload;
			var errors = new List<string>();
			
			// Validate registrations and return at that point if any failures occur.
			var parameterValidateResult = _parameterValidatorService.Validate(payload);
			if (!parameterValidateResult.IsSuccess)
			{
				errors.AddRange(parameterValidateResult.ValidationErrors);
			}

			return new PayloadValidationResult(errors);
		}
	}
}