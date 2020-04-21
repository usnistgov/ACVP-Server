using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class DeleteImplementationPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IImplementationService _implementationService;

		public DeleteImplementationPayloadValidator(IImplementationService implementationService)
		{
			_implementationService = implementationService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			DeleteParameters parameters = ((DeletePayload)workflowItemPayload).ToDeleteParameters();

			//Verify that the implementation exists
			if (!_implementationService.ImplementationExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Implementation {parameters.ID} does not exist");
			}

			//Not really anything else to validate...

			return true;
		}
	}
}
