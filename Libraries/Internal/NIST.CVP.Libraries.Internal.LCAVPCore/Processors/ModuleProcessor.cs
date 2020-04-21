using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.ACVPCore;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public class ModuleProcessor : IModuleProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IAddressService _addressService;
		private readonly IOrganizationService _organizationService;
		private readonly IPersonProcessor _personProcessor;

		public ModuleProcessor(IImplementationService implementationService, IAddressService addressService, IOrganizationService organizationService, IPersonProcessor personProcessor)
		{
			_implementationService = implementationService;
			_addressService = addressService;
			_organizationService = organizationService;
			_personProcessor = personProcessor;
		}

		public InsertResult Create(Module module)
		{
			//Need to create the organization, and the people for contacts. Try to be smart and look for them?

			OrganizationProcessor organizationProcessor = new OrganizationProcessor(_organizationService, _addressService);
			var organizationCreateResult = organizationProcessor.Create(module.Vendor);

			//Need to get the address that was created as part of creating the org
			long addressID = _addressService.GetAllForOrganization(organizationCreateResult.ID).First().ID;

			//Create the people who are the contacts
			List<long> contactIDs = new List<long>();
			foreach (Contact contact in module.Contacts)
			{
				//Need to put the newly created org ID on the contact object so it can be created
				contact.OrganizationID = organizationCreateResult.ID;

				var result = _personProcessor.Create(contact);
				contactIDs.Add(result.ID);
			}

			ImplementationCreateParameters createParameters = new ImplementationCreateParameters
			{
				OrganizationID = organizationCreateResult.ID,
				AddressID = addressID,
				Name = module.Name,
				Description = module.Description,
				Version = module.Version,
				Type = ImplementationTypeExtensions.FromString(module.Type),
				ContactIDs = contactIDs,
				IsITAR = false,
			};

			return _implementationService.Create(createParameters);
		}

		public void Update(Module module)
		{
			ImplementationUpdateParameters updateParameters = new ImplementationUpdateParameters
			{
				ID = module.ID,
				Name = module.Name,
				NameUpdated = module.NameUpdated,
				Description = module.Description,
				DescriptionUpdated = module.DescriptionUpdated,
				Version = module.Version,
				VersionUpdated = module.VersionUpdated,
				Type = ImplementationTypeExtensions.FromString(module.Type),
				TypeUpdated = module.TypeUpdated,
				//The rest can't be updated via CAVS
				WebsiteUpdated = false,
				OrganizationIDUpdated = false,
				AddressIDUpdated = false,
				ContactIDsUpdated = false
			};

			_implementationService.Update(updateParameters);


			//Contact updates are handled through Person updates
			//Contact additions have to be handled here, but they're incompatible with how the implementation update works, so they have to be done differently
			if (module.Contact1Added || module.Contact2Added)
			{
				//Contact creation means Person creation, which requires an organization that we don't have. So need to look up the Implementation to get its org id
				long organizationID = _implementationService.Get(module.ID).Vendor.ID;

				if (module.Contact1Added)
				{
					module.Contacts[0].OrganizationID = organizationID;
					var result = _personProcessor.Create(module.Contacts[0]);

					_implementationService.AddContact(module.ID, result.ID, 0);
				}

				if (module.Contact2Added)
				{
					module.Contacts[module.Contacts.Count - 1].OrganizationID = organizationID;
					var result = _personProcessor.Create(module.Contacts[module.Contacts.Count - 1]);       //The collection will only have 1 contact in it if only contact 2 was added, so take whatever the last contact is

					_implementationService.AddContact(module.ID, result.ID, 1);
				}
			}
		}
	}
}
