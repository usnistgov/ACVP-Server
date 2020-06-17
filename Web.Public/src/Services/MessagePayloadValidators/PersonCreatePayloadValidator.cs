using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class PersonCreatePayloadValidator : IMessagePayloadValidator
	{
		private readonly IOrganizationService _organizationService;

		public PersonCreatePayloadValidator(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (PersonCreatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (string.IsNullOrEmpty(item.Name))
			{
				errors.Add("person.name must be provided.");
			}

			if (string.IsNullOrEmpty(item.OrganizationURL))
			{
				errors.Add("person.vendorUrl must be provided.");
			}
			else
			{
				if (!_organizationService.Exists(BasePayload.ParseIDFromURL(item.OrganizationURL)))
				{
					errors.Add("person.vendorUrl is invalid.");
				}
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}