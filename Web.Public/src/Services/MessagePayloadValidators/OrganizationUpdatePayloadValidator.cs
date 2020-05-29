using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Results;

namespace Web.Public.Services.MessagePayloadValidators
{
	public class OrganizationUpdatePayloadValidator : IMessagePayloadValidator
	{
		private readonly IOrganizationService _organizationService;
		private readonly IAddressService _addressService;

		public OrganizationUpdatePayloadValidator(IOrganizationService organizationService, IAddressService addressService)
		{
			_organizationService = organizationService;
			_addressService = addressService;
		}
		
		public PayloadValidationResult Validate(IMessagePayload workflowItemPayload)
		{
			var item = (OrganizationUpdatePayload) workflowItemPayload;
			var errors = new List<string>();

			if (!_organizationService.Exists(item.ID))
			{
				errors.Add("vendor.id is not valid.");
			}
			
			if (item.NameUpdated && string.IsNullOrEmpty(item.Name))
			{
				errors.Add("vendor.name when provided in an update must not be empty or null.");
			}

			if (item.AddressesUpdated && item.Addresses?.Count == 0)
			{
				errors.Add("vendor.addresses must provide at least one address.");
			}
			
			if (item.Addresses?.Count > 0)
			{
				for (var i = 0; i < item.Addresses.Count; i++)
				{
					var addressId = BasePayload.ParseNullableIDFromURL(item.Addresses[i].URL);

					// if not a new address, validate it exists
					if (addressId != null)
					{
						if (_addressService.Get(item.ID, addressId.Value) == null)
						{
							errors.Add($"vendor.addresses[{i}] {item.Addresses[i].URL} is invalid.");
						}
					}
				}
			}
			
			return new PayloadValidationResult(errors);
		}
	}
}