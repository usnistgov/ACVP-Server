using System;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Exceptions;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class DependencyUpdatePayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IDependencyService _dependencyService;

		public DependencyUpdatePayloadValidator(IDependencyService dependencyService)
		{
			_dependencyService = dependencyService;
		}
		
		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (DependencyUpdatePayload) workflowItemPayload;

			if (_dependencyService.GetDependency(item.ID) == null)
			{
				throw new JsonReaderException("Dependency does not exist.");
			}
			
			if (item.NameUpdated && string.IsNullOrEmpty(item.Name))
			{
				throw new JsonReaderException("dependency.name cannot be updated to an empty or null value.");
			}
			
			return true;
		}
	}
}