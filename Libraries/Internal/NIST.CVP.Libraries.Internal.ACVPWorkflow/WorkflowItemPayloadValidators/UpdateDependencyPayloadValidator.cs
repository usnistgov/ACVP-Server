using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class UpdateDependencyPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IDependencyService _dependencyService;

		public UpdateDependencyPayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			DependencyUpdateParameters parameters = ((DependencyUpdatePayload)workflowItemPayload).ToDependencyUpdateParameters();

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
