using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class ImplementationCreatePayloadValidator : IMessagePayloadValidator
	{
		private readonly IAddressService _addressService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonService _personService;

		public ImplementationCreatePayloadValidator(IAddressService addressService, IOrganizationService organizationService, IPersonService personService)
		{
			_addressService = addressService;
			_organizationService = organizationService;
			_personService = personService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (ImplementationCreatePayload) workflowItemPayload;
			var errors = new List<string>();
			
			if (string.IsNullOrEmpty(item.Name))
			{
				errors.Add("module.name must be provided.");
			}

			if (string.IsNullOrEmpty(item.VendorURL))
			{
				errors.Add("module.vendorUrl must be provided.");
			}

			var vendor = _organizationService.Get(BasePayload.ParseIDFromURL(item.VendorURL));
			if (vendor == null)
			{
				errors.Add("module.vendorUrl not valid.");
			}
			
			if (string.IsNullOrEmpty(item.AddressURL))
			{
				errors.Add("module.addressUrl must be provided.");
			}
			else
			{
				if (vendor != null)
				{
					var address = _addressService.Get(vendor.ID, BasePayload.ParseIDFromURL(item.AddressURL));
					if (address == null)
						errors.Add("module.addressUrl not valid.");
				}
			}
			
			if (string.IsNullOrEmpty(item.Type))
			{
				errors.Add("module.type must be provided.");
			}

			if (item.ContactURLs == null)
			{
				errors.Add("module.contactUrls must be provided.");
			}
			else
			{
				foreach (var contactUrl in item.ContactURLs)
				{
					if (_personService.Get(BasePayload.ParseIDFromURL(contactUrl)) == null)
						errors.Add($"module.contactUrl {contactUrl} does not exist.");
				}				
			}

			if (string.IsNullOrEmpty(item.Description))
			{
				errors.Add("module.description must be provided.");
			}

			if (string.IsNullOrEmpty(item.Version))
			{
				errors.Add("module.version must be provided.");
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}