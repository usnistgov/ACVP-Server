using System.Collections.Generic;
using Autofac;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class ImplementationDeletePayloadValidator : IWorkflowItemValidator
	{
		private readonly IImplementationService _implementationService;

		public ImplementationDeletePayloadValidator(IImplementationService implementationService)
		{
			_implementationService = implementationService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (DeletePayload) workflowItemPayload;
			var errors = new List<string>();
			
			// if implementation !exists
			if (!_implementationService.Exists(item.ID))
			{
				errors.Add("module.id is invalid.");
				return new PayloadValidationResult(errors);
			}

			if (_implementationService.IsUsed(item.ID))
			{
				errors.Add("module is in use and cannot be deleted.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}