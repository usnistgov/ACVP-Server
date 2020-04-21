using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class DeleteDependencyPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IDependencyService _dependencyService;

		public DeleteDependencyPayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			DeleteParameters parameters = ((DeletePayload)workflowItemPayload).ToDeleteParameters();

			//Verify that the dependency exists
			if (!_dependencyService.DependencyExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Dependency {parameters.ID} does not exist");
			}

			//Not really anything else to validate...

			return true;
		}
	}
}
