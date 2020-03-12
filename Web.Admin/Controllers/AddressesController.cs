using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AddressesController : ControllerBase
	{
		private readonly IAddressService _addressService;

		public AddressesController(IAddressService addressService)
		{
			_addressService = addressService;
		}

		[HttpPost]
		public InsertResult CreatePerson(AddressCreateParameters parameters)
		{
			return _addressService.Create(parameters);
		}


		[HttpPatch("{addressId}")]
		public Result UpdateAddress(long addressId, Address address)
		{
			AddressUpdateParameters parameters = new AddressUpdateParameters();

			if (address.Street1 != null)
			{
				parameters.Street1 = address.Street1;
				parameters.Street1Updated = true;
			}
			if (address.Street2 != null)
			{
				parameters.Street2 = address.Street2;
				parameters.Street2Updated = true;
			}
			if (address.Street3 != null)
			{
				parameters.Street3 = address.Street3;
				parameters.Street3Updated = true;
			}
			if (address.Locality != null)
			{
				parameters.Locality = address.Locality;
				parameters.LocalityUpdated = true;
			}
			if (address.Region != null)
			{
				parameters.Region = address.Region;
				parameters.RegionUpdated = true;
			}
			if (address.Country != null)
			{
				parameters.Country = address.Country;
				parameters.CountryUpdated = true;
			}
			if (address.PostalCode != null)
			{
				parameters.PostalCode = address.PostalCode;
				parameters.PostalCodeUpdated = true;
			}
			parameters.ID = addressId;
			return _addressService.Update(parameters);
		}
	}
}