using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IWorkflowService _workflowService;
		private readonly IDependencyService _dependencyService;

		public UpdateOEProcessor(IOEService oeService, IDependencyService dependencyService, IWorkflowService workflowService)
		{
			_oeService = oeService;
			_dependencyService = dependencyService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
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

			//Update the workflow item
			if (oeUpdateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, oeUpdateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
