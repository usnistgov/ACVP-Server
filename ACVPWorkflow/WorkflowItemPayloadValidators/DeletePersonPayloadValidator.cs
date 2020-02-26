using ACVPCore.Models.Parameters;
using ACVPCore.Services;
using ACVPWorkflow.Exceptions;
using ACVPWorkflow.Models;

namespace ACVPWorkflow.WorkflowItemPayloadValidators
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
