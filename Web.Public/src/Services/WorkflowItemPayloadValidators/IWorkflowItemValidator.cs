using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public interface IWorkflowItemValidator
	{
		PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload);
	}
}