using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Exceptions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.WorkflowItemPayloadValidators
{
	public class DeletePersonPayloadValidator : IWorkflowItemPayloadValidator
	{
		private readonly IPersonService _personService;

		public DeletePersonPayloadValidator(IPersonService personService)
		{
			_personService = personService;
		}

		public bool Validate(IWorkflowItemPayload workflowItemPayload)
		{
			DeleteParameters parameters = ((DeletePayload)workflowItemPayload).ToDeleteParameters();

			//Verify that the person exists
			if (!_personService.PersonExists(parameters.ID))
			{
				throw new ResourceDoesNotExistException($"Person {parameters.ID} does not exist");
			}

			//Not really anything else to validate...

			return true;
		}
	}
}
