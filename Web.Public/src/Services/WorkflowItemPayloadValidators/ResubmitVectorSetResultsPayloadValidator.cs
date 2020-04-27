using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class ResubmitVectorSetResultsPayloadValidator : IWorkflowItemValidator
	{
		private readonly IVectorSetService _vectorSetService;

		public ResubmitVectorSetResultsPayloadValidator(IVectorSetService vectorSetService)
		{
			_vectorSetService = vectorSetService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			//var payload = 
			
			// Check prompt exists
		
			// Environment check done by controller
			
			throw new System.NotImplementedException();
		}
	}
}