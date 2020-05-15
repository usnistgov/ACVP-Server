using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class DependencyUpdatePayloadValidator : IMessagePayloadValidator
	{
		private readonly IDependencyService _dependencyService;

		public DependencyUpdatePayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (DependencyUpdatePayload) workflowItemPayload;
			var errors = new List<string>();
			
			if (_dependencyService.GetDependency(item.ID) == null)
			{
				errors.Add("Dependency does not exist.");
			}
			
			if (item.NameUpdated && string.IsNullOrEmpty(item.Name))
			{
				errors.Add("dependency.name cannot be updated to an empty or null value.");
			}

			return new PayloadValidationResult(errors);
		}
	}
}