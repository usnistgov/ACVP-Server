using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class OperatingEnvironmentUpdatePayloadValidator : IWorkflowItemValidator
	{
		private readonly IWorkflowItemValidatorFactory _workflowItemValidatorFactory;
		private readonly IDependencyService _dependencyService;

		public OperatingEnvironmentUpdatePayloadValidator(IWorkflowItemValidatorFactory workflowItemValidatorFactory, IDependencyService dependencyService)
		{
			_workflowItemValidatorFactory = workflowItemValidatorFactory;
			_dependencyService = dependencyService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (OEUpdatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (item.NameUpdated && string.IsNullOrEmpty(item.Name))
			{
				errors.Add("oes.name must be provided.");
			}

			if (item.DependenciesUpdated)
			{
				if (item.DependencyURLs?.Count == 0 && item.DependenciesToCreate?.Count == 0)
				{
					errors.Add("oes must provide dependencyUrls and/or dependencies.");
				}
				
				if (item.DependenciesToCreate?.Count > 0)
				{
					var index = 0;
					foreach (var dependencyToCreate in item.DependenciesToCreate)
					{
						var payloadValidationResult = _workflowItemValidatorFactory
							.GetWorkflowItemPayloadValidator(APIAction.CreateDependency)
							.Validate(dependencyToCreate);

						foreach (var error in payloadValidationResult.Errors)
						{
							errors.Add($"oes.dependencies[{index}]: {error}");
						}
						index++;
					}
				}

				if (item.DependencyURLs?.Count > 0)
				{
					foreach (var url in item.DependencyURLs)
					{
						if (!_dependencyService.Exists(BasePayload.ParseIDFromURL(url)))
						{
							errors.Add($"oes.dependencyUrl {url} is invalid.");
						}
					}
				}
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}