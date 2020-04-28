using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.WorkflowItemPayloadValidators
{
	public class OrganizationDeletePayloadValidator : IWorkflowItemValidator
	{
		private readonly IOrganizationService _organizationService;

		public OrganizationDeletePayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}
		
		public PayloadValidationResult Validate(IWorkflowItemPayload workflowItemPayload)
		{
			var item = (DeletePayload) workflowItemPayload;
			var errors = new List<string>();
			
			if (!_organizationService.Exists(item.ID))
			{
				errors.Add("vendor.id is invalid.");
				return new PayloadValidationResult(errors);
			}

			if (_organizationService.IsUsed(item.ID))
			{
				errors.Add("vendor is in use and cannot be deleted.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}