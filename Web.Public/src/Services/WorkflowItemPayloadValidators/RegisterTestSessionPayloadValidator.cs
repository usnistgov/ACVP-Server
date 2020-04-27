using System.Collections.Generic;
using System.Linq;
using System.Net;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.JsonObjects;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class RegisterTestSessionPayloadValidator : IWorkflowItemValidator
	{
		private readonly IParameterValidatorService _parameterValidatorService;

		public RegisterTestSessionPayloadValidator(IParameterValidatorService parameterValidatorService)
		{
			_parameterValidatorService = parameterValidatorService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
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