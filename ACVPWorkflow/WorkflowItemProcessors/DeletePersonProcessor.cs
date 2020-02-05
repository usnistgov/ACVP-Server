using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;
using ACVPWorkflow.Services;

namespace ACVPWorkflow.WorkflowItemProcessors
{
	public class DeletePersonProcessor : IWorkflowItemProcessor
	{
		private readonly IPersonService _personService;

		public DeletePersonProcessor(IPersonService personService)
		{
			_personService = personService;
		}

		public long Approve(WorkflowItem workflowItem)
		{
			//Deserialize the JSON
			DeleteParameters deleteParameters = ((DeletePayload)workflowItem.Payload).ToDeleteParameters();

			//Delete that person - will fail if person is in use
			DeleteResult deleteResult = _personService.Delete(deleteParameters.ID);

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
