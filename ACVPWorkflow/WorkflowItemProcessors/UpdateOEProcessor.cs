using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IDependencyService _dependencyService;

		public UpdateOEProcessor(IOEService oeService, IDependencyService dependencyService)
		{
			_oeService = oeService;
			_dependencyService = dependencyService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			OEUpdatePayload oeUpdatePayload = (OEUpdatePayload)workflowItem.Payload;
			OEUpdateParameters parameters = oeUpdatePayload.ToOEUpdateParameters();

			//If there were any new Dependencies in the OE, instead of just URLs, create those and add them to the collection of dependency IDs
			foreach (DependencyCreatePayload dependencyCreatePayload in oeUpdatePayload.DependenciesToCreate)
			{
				//Convert from a payload to parameters
				DependencyCreateParameters dependencyCreateParameters = dependencyCreatePayload.ToDependencyCreateParameters();

				//Create it
				DependencyResult dependencyCreateResult = _dependencyService.Create(dependencyCreateParameters);

				//Add it to the dependency list
				if (dependencyCreateResult.IsSuccess)
				{
					parameters.DependencyIDs.Add(dependencyCreateResult.ID);
				}
			}

			//Update it
			OEResult oeUpdateResult = _oeService.Update(parameters);

			if (!oeUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return oeUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
