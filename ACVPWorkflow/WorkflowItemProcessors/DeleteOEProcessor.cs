using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeleteOEProcessor : IWorkflowItemProcessor
	{
		private readonly IOEService _oeService;

		public DeleteOEProcessor(IOEService oeService)
		{
			_oeService = oeService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Deserialize the JSON
			DeleteParameters deleteParameters = ((DeletePayload)workflowItem.Payload).ToDeleteParameters();

			//Delete that oe - will fail if oe is in use
			DeleteResult deleteResult = _oeService.Delete(deleteParameters.ID);

			if (deleteResult.IsInUse)
			{
				throw new ResourceInUseException(
					$"The resource could not be deleted as other resources are referencing it. Workflow Info: {workflowItem.APIAction} {workflowItem.WorkflowItemID}");
			}
			
			if (!deleteResult.IsSuccess)
			{
				//Mark as error? No way to do so, just in process
				// TODO - something here
				throw new ResourceProcessorException(
					$"The resource could not be deleted. Workflow Info: {workflowItem.APIAction} {workflowItem.WorkflowItemID}");				
			}
			
			return deleteParameters.ID;
		}

		public void Reject(WorkflowItem workflowItem) { }
	}
}
