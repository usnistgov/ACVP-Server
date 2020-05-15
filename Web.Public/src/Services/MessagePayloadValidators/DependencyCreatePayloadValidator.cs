using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class DependencyCreatePayloadValidator : IMessagePayloadValidator
	{
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (DependencyCreatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (string.IsNullOrEmpty(item.Name))
			{
				errors.Add("dependency.name must be provided.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}