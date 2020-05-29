using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class PersonDeletePayloadValidator : IMessagePayloadValidator
	{
		private readonly IPersonService _personService;

		public PersonDeletePayloadValidator(IPersonService personService)
		{
			_personService = personService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
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