using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class ImplementationUpdatePayloadValidator : IMessagePayloadValidator
	{
		private readonly IImplementationService _implementationService;
		private readonly IAddressService _addressService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;

		public ImplementationUpdatePayloadValidator(IImplementationService implementationService, IAddressService addressService, IOrganizationService organizationService, IPersonService personService)
		{
			_implementationService = implementationService;
			_addressService = addressService;
			_organizationService = organizationService;
			_personService = personService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (ImplementationUpdatePayload) workflowItemPayload;
			var errors = new List<string>();

			var currentImplementation = _implementationService.GetImplementation(item.ID);

			if (currentImplementation == null)
			{
				errors.Add("module.id is invalid.");
				return new PayloadValidationResult(errors);
			}

			if (item.DescriptionUpdated && string.IsNullOrEmpty(item.Description))
			{
				errors.Add("module.description cannot be null or empty.");
			}

			if (item.TypeUpdated && string.IsNullOrEmpty(item.Type))
			{
				errors.Add("module.type cannot be null or empty.");
			}

			if (item.VersionUpdated && string.IsNullOrEmpty(item.Version))
			{
				errors.Add("module.version cannot be null or empty.");
			}

			var orgId = currentImplementation.OrganizationID;
			if (item.VendorURLUpdated)
			{
				orgId = BasePayload.ParseIDFromURL(item.VendorURL);
				if (!_organizationService.Exists(orgId))
				{
					errors.Add("module.vendorUrl is invalid.");
				}
			}
			
			if (item.AddressURLUpdated && _addressService.Get(orgId,BasePayload.ParseIDFromURL(item.AddressURL)) == null)
			{
				errors.Add("module.addressUrl is invalid.");
			}

			if (item.ContactURLsUpdated)
			{
				foreach (var contactUrl in item.ContactURLs)
				{
					if (!_personService.Exists(BasePayload.ParseIDFromURL(contactUrl)))
					{
						errors.Add($"module.contactUrl {contactUrl} is invalid.");
					}
				}
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}