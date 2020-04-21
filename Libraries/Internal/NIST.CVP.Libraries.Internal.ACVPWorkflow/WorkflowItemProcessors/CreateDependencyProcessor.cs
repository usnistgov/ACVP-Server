using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Results;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemProcessors
{
	public class CreateDependencyProcessor : BaseWorkflowItemProcessor, IWorkflowItemProcessor
	{
		private readonly IDependencyService _dependencyService; private IWorkflowItemPayloadValidatorFactory _workflowItemPayloadValidatorFactory;

		public CreateDependencyProcessor(IDependencyService dependencyService, IWorkflowItemPayloadValidatorFactory workflowItemPayloadValidatorFactory)
		{
			_dependencyService = dependencyService;
			_workflowItemPayloadValidatorFactory = workflowItemPayloadValidatorFactory;
		}

		public bool Validate(WorkflowItem workflowItem)
		{
			return IsPendingApproval(workflowItem) && _workflowItemPayloadValidatorFactory.GetWorkflowItemPayloadValidator(APIAction.CreateDependency).Validate((DependencyCreatePayload)workflowItem.Payload);
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Validate this workflow item
			Validate(workflowItem);

			DependencyCreateParameters parameters = ((DependencyCreatePayload)workflowItem.Payload).ToDependencyCreateParameters();

			//Create it
			DependencyResult dependencyCreateResult = _dependencyService.Create(parameters);

			if (!dependencyCreateResult.IsSuccess)
			{
				throw new ResourceProcessorException($"Failed approval on {nameof(workflowItem.APIAction)} {workflowItem.APIAction}");
			}
			
			return dependencyCreateResult.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
