using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class UpdateOEPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOEService _oeService;
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;


		public UpdateOEPayloadValidator(IOEService oeService, IDependencyService dependencyService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_oeService = oeService;
			_dependencyService = dependencyService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			OEUpdatePayload oeUpdatePayload = (OEUpdatePayload)workflowItemPayload;
			OEUpdateParameters parameters = oeUpdatePayload.ToOEUpdateParameters();

			//Verify that the OE exists
			if (!_oeService.OEExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"OE {parameters.ID} does not exist");
			}

			//Verify that each of the referenced Dependencies exists
			foreach (long dependencyID in parameters.DependencyIDs)
			{
				if (!_dependencyService.DependencyExists(dependencyID))
				{
					throw new ResourceDoesNotExistException($"Dependency {dependencyID} does not exist");
				}
			}

			//If there are any inline dependencies, validate those
			if (oeUpdatePayload.DependenciesToCreate.Count > 0)
			{
				var dependencyValidator = _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateDependency);
				foreach (DependencyCreatePayload dependencyCreatePayload in oeUpdatePayload.DependenciesToCreate)
				{
					dependencyValidator.Validate(dependencyCreatePayload);
				}
			}

			return true;
		}
	}
}
