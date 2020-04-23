using Newtonsoft.Json;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class DependencyDeletePayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IDependencyService _dependencyService;

		public DependencyDeletePayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (DeletePayload) workflowItemPayload;

			if (_dependencyService.GetDependency(item.ID) == null)
			{
				throw new JsonReaderException("Dependency does not exist.");
			}

			return true;
		}
	}
}