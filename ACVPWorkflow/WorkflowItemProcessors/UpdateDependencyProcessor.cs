using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class UpdateDependencyProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService;
		private IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public UpdateDependencyProcessor(IDependencyService dependencyService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_dependencyService = dependencyService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.UpdateDependency).Validate((DependencyUpdatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			DependencyUpdateParameters parameters = ((DependencyUpdatePayload)workflowItem.Payload).ToDependencyUpdateParameters();

			//Update it
			DependencyResult dependencyUpdateResult = _dependencyService.Update(parameters);

			if (!dependencyUpdateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}

			return dependencyUpdateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
