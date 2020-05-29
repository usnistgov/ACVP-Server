using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class OperatingEnvironmentDeletePayloadValidator : IMessagePayloadValidator
	{
		private IOEService _oeService;

		public OperatingEnvironmentDeletePayloadValidator(IOEService oeService)
		{
			_oeService = oeService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (DeletePayload) workflowItemPayload;
			var errors = new List<string>();
			
			// if implementation !exists
			if (!_oeService.Exists(item.ID))
			{
				errors.Add("oe.id is invalid.");
				return new PayloadValidationResult(errors);
			}

			if (_oeService.IsUsed(item.ID))
			{
				errors.Add("OE is in use and cannot be deleted.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}