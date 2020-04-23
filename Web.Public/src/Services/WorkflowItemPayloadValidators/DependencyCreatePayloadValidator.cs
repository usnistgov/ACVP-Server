using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class DependencyCreatePayloadValidator : IWorkflowItemValidator
	{
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
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