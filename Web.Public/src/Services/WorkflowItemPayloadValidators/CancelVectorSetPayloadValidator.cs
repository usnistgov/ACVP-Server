using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class CancelVectorSetPayloadValidator : IWorkflowItemValidator
	{
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			// TODO can you cancel something that has already been published?
			var payload = (CancelPayload) workflowItemPayload;

			// Check that the vector set exists and is an element of the test session is done by JWT claims
			return new PayloadValidationResult(new List<string>());
		}
	}
}