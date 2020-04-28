using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class PersonCreatePayloadValidator : IWorkflowItemValidator
	{
		private readonly IOrganizationService _organizationService;

		public PersonCreatePayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (PersonCreatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (string.IsNullOrEmpty(item.Name))
			{
				errors.Add("person.name must be provided.");
			}

			if (!_organizationService.Exists(BasePayload.ParseIDFromURL(item.OrganizationURL)))
			{
				errors.Add("person.vendorUrl is invalid.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}