using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class DeleteOEPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IOEService _oeService;

		public DeleteOEPayloadValidator(IOEService oeService)
		{
			_oeService = oeService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			DeleteParameters parameters = ((DeletePayload)workflowItemPayload).ToDeleteParameters();

			//Verify that the OE exists
			if (!_oeService.OEExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"OE {parameters.ID} does not exist");
			}

			//Not really anything else to validate...

			return true;
		}
	}
}
