using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class DependencyDeletePayloadValidator : IWorkflowItemValidator
	{
		private readonly IDependencyService _dependencyService;

		public DependencyDeletePayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
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