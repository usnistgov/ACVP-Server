﻿using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class CreateOEPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CreateOEPayloadValidator(IDependencyService dependencyService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_dependencyService = dependencyService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			OECreatePayload oeCreatePayload = (OECreatePayload)workflowItemPayload;
			OECreateParameters parameters = oeCreatePayload.ToOECreateParameters();

			//Verify that the dependencies exist
			foreach (long dependencyID in parameters.DependencyIDs)
			{
				if (!_dependencyService.DependencyExists(dependencyID))
				{
					throw new ResourceDoesNotExistException($"Dependency {dependencyID} does not exist");
				}
			}

			//If there are any inline dependencies, validate those
			if (oeCreatePayload.DependenciesToCreate.Count > 0)
			{
				var dependencyValidator = _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateDependency);
				foreach (DependencyCreatePayload dependencyCreatePayload in oeCreatePayload.DependenciesToCreate)
				{
					dependencyValidator.Validate(dependencyCreatePayload);
				}
			}

			return true;
		}
	}
}