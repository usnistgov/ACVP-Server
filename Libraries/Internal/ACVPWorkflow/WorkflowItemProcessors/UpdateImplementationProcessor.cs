using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateImplementationProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public UpdateImplementationProcessor(IImplementationService implementationService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_implementationService = implementationService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.UpdateImplementation).Validate((ImplementationUpdatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			ImplementationUpdateParameters parameters = ((ImplementationUpdatePayload)workflowItem.Payload).ToImplementationUpdateParameters();

			//Update it
			ImplementationResult implementationUpdateResult = _implementationService.Update(parameters);

			if (!implementationUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return implementationUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
