using System.Text.Json;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class DependencyCreatePayloadValidator : IWorkflowItemPayloadValidator
	{
		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (DependencyCreatePayload) workflowItemPayload;

			if (string.IsNullOrEmpty(item.Name))
			{
				throw new JsonException("dependency.name must be provided.");
			}
			
			return true;
		}
	}
}