using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class CancelTestSessionPayloadValidator : IWorkflowItemValidator
	{
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			// TODO can you cancel something that has already been published?
			
			// Check that the test session exists is done by JWT claims
			return new PayloadValidationResult(new List<string>());
		}
	}
}