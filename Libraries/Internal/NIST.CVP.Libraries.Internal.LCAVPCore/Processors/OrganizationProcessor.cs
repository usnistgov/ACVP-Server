using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public class OrganizationProcessor : IOrganizationProcessor
	{
		private readonly IOrganizationService _organizationService;
		private readonly IAddressService _addressService;

		public OrganizationProcessor(IOrganizationService organizationService, IAddressService addressService)
		{
			_organizationService = organizationService;
			_addressService = addressService;
		}

		public InsertResult Create(Vendor organization)
		{

			OrganizationCreateParameters createParameters = new OrganizationCreateParameters
			{
				Name = organization.Name,
				Website = organization.Website,
				Addresses = new List<AddressCreateParameters>{
					new AddressCreateParameters {
						Street1 = organization.Address[0].Street1,
						Street2 = organization.Address[0].Street2,
						Street3 = organization.Address[0].Street3,
						Locality = organization.Address[0].Locality,
						Region = organization.Address[0].Region,
						Country = organization.Address[0].Country,
						PostalCode = organization.Address[0].PostalCode
					}
				}
			};

			return _organizationService.Create(createParameters);
		}

		public void Update(Vendor organization)
		{

			OrganizationUpdateParameters updateParameters = new OrganizationUpdateParameters
			{
				ID = organization.ID,
			};

			if (organization.NameUpdated)
			{
				updateParameters.Name = organization.Name;
				updateParameters.NameUpdated = true;
			}

			if (organization.WebsiteUpdated)
			{
				updateParameters.Website = organization.Website;
				updateParameters.WebsiteUpdated = true;
			}


			if (organization.AddressUpdated)
			{
				updateParameters.AddressesUpdated = true;
				var foo = organization.Address[0];

				AddressUpdateParameters addressUpdateParameters = new AddressUpdateParameters
				{
					ID = foo.ID,
					OrderIndex = 0,     //Because only have 1 if coming this way...
					OrganizationID = foo.VendorID       //This won't get used, but still populating it since I have it
				};

				if (foo.Street1Updated) { addressUpdateParameters.Street1 = foo.Street1; addressUpdateParameters.Street1Updated = true; }
				if (foo.Street2Updated) { addressUpdateParameters.Street2 = foo.Street2; addressUpdateParameters.Street2Updated = true; }
				if (foo.Street3Updated) { addressUpdateParameters.Street3 = foo.Street3; addressUpdateParameters.Street3Updated = true; }
				if (foo.LocalityUpdated) { addressUpdateParameters.Locality = foo.Locality; addressUpdateParameters.LocalityUpdated = true; }
				if (foo.RegionUpdated) { addressUpdateParameters.Region = foo.Region; addressUpdateParameters.RegionUpdated = true; }
				if (foo.CountryUpdated) { addressUpdateParameters.Country = foo.Country; addressUpdateParameters.CountryUpdated = true; }
				if (foo.PostalCodeUpdated) { addressUpdateParameters.PostalCode = foo.PostalCode; addressUpdateParameters.PostalCodeUpdated = true; }

				updateParameters.Addresses = new List<object> { addressUpdateParameters };
			}

			_organizationService.Update(updateParameters);
		}
	}
}
