using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class OrganizationDeletePayloadValidator : IMessagePayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public OrganizationDeletePayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
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