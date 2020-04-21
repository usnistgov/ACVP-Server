using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Exceptions;
using NIST.CVP.Libraries.Internal.ACVPWorkflow.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
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
