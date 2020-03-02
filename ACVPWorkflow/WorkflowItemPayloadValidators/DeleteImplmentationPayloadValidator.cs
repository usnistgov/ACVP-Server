using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
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
