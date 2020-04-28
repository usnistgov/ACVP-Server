using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class PersonUpdatePayloadValidator : IWorkflowItemValidator
	{
		private readonly IPersonService _personService;
		private readonly IOrganizationService _organizationService;
		
		public PersonUpdatePayloadValidator(IPersonService personService, IOrganizationService organizationService)
		{
			_personService = personService;
			_organizationService = organizationService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (PersonUpdatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (!_personService.Exists(item.ID))
			{
				errors.Add("person.id is not valid.");
			}
			
			if (item.NameUpdated && string.IsNullOrEmpty(item.Name))
			{
				errors.Add("person.name must be provided.");
			}

			if (item.OrganizationURLUpdated && !_organizationService.Exists(BasePayload.ParseIDFromURL(item.OrganizationURL)))
			{
				errors.Add("person.vendorUrl is invalid.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}