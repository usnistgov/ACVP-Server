using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
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
