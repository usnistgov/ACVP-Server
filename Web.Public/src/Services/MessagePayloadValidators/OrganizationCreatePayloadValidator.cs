using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class OrganizationCreatePayloadValidator : IMessagePayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public OrganizationCreatePayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (OrganizationCreatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (string.IsNullOrEmpty(item.Name))
			{
				errors.Add("vendor.name must be provided.");
			}

			if (!string.IsNullOrEmpty(item.ParentURL))
			{
				if (!_organizationService.Exists(BasePayload.ParseIDFromURL(item.ParentURL)))
				{
					errors.Add("vendor.link is invalid.");
				}
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}