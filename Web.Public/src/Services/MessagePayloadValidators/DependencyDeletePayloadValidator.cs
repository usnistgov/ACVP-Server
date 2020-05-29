using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class DependencyDeletePayloadValidator : IMessagePayloadValidator
	{
		private readonly IDependencyService _dependencyService;

		public DependencyDeletePayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (DeletePayload) workflowItemPayload;
			var errors = new List<string>();

			if (_dependencyService.GetDependency(item.ID) == null)
			{
				errors.Add("Dependency does not exist.");
			}

			return new PayloadValidationResult(errors);
		}
	}
}