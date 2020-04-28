using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class PersonDeletePayloadValidator : IWorkflowItemValidator
	{
		private readonly IPersonService _personService;

		public PersonDeletePayloadValidator(IPersonService personService)
		{
			_personService = personService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (DeletePayload) workflowItemPayload;
			var errors = new List<string>();
			
			if (!_personService.Exists(item.ID))
			{
				errors.Add("person.id is invalid.");
				
			}

			return new PayloadValidationResult(errors);
		}
	}
}