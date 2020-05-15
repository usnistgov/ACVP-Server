using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class ImplementationDeletePayloadValidator : IMessagePayloadValidator
	{
		private readonly IImplementationService _implementationService;

		public ImplementationDeletePayloadValidator(IImplementationService implementationService)
		{
			_implementationService = implementationService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
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