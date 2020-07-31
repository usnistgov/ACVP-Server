using System;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateOEProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;
		private readonly IDependencyService _dependencyService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CreateOEProcessor(IOEService oeService, IDependencyService dependencyService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_oeService = oeService;
			_dependencyService = dependencyService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateOE).Validate((OECreatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			OECreatePayload oeCreatePayload = (OECreatePayload)workflowItem.Payload;
			OECreateParameters parameters = oeCreatePayload.ToOECreateParameters();
			parameters.IsITAR = false;	//TODO - something for ITARs, for now assume no ITARs

			//If there were any new Dependencies in the OE, instead of just URLs, create those and add them to the collection of dependency IDs
			if (oeCreatePayload.DependenciesToCreate != null)
			{
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
			}

			//Create it
			OEResult result = _oeService.Create(parameters);

			if (!result.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}.{Environment.NewLine}Reason: {result.ErrorMessage}");
			}
			
			return result.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
