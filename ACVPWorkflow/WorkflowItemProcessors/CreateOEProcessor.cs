using System;
using System.Text.Json;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IWorkflowService _workflowService;
		private readonly IDependencyService _dependencyService;

		public CreateOEProcessor(IOEService oeService, IDependencyService dependencyService, IWorkflowService workflowService)
		{
			_oeService = oeService;
			_dependencyService = dependencyService;
			_workflowService = workflowService;
		}

		public void Approve(WorkflowItem workflowItem)
		{
			OECreatePayload oeCreatePayload = (OECreatePayload)workflowItem.Payload;
			OECreateParameters parameters = oeCreatePayload.ToOECreateParameters();

			//If there were any new Dependencies in the OE, instead of just URLs, create those and add them to the collection of dependency IDs
			foreach (DependencyCreatePayload dependencyCreatePayload in oeCreatePayload.DependenciesToCreate)
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

			//Create it
			OEResult oeCreateResult = _oeService.Create(parameters);

			//Update the workflow item
			if (oeCreateResult.IsSuccess)
			{
				_workflowService.MarkApproved(workflowItem.WorkflowItemID, oeCreateResult.ID);
			}
		}

		public void Reject(WorkflowItem workflowItem)
		{
			throw new NotImplementedException();
		}
	}
}
