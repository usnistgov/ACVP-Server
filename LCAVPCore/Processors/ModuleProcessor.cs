using System.Collections.Generic;
using System.Linq;
using ACVPCore;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public class ModuleProcessor : IModuleProcessor
	{
		private readonly IImplementationService _implementationService;
		private readonly IPersonService _personService;
		private readonly IAddressService _addressService;
		private readonly IOrganizationService _organizationService;

		public ModuleProcessor(IImplementationService implementationService, IPersonService personService, IAddressService addressService, IOrganizationService organizationService)
		{
			_implementationService = implementationService;
			_personService = personService;
			_addressService = addressService;
			_organizationService = organizationService;
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
				var result = new PersonProcessor(_personService).Create(contact);
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
			if (module.Contact1Added)
			{
				var result = new PersonProcessor(_personService).Create(module.Contacts[0]);

				_implementationService.AddContact(module.ID, result.ID, 0);
			}

			if (module.Contact2Added)
			{
				var result = new PersonProcessor(_personService).Create(module.Contacts[module.Contacts.Count - 1]);		//The collection will only have 1 contact in it if only contact 2 was added, so take whatever the last contact is

				_implementationService.AddContact(module.ID, result.ID, 1);
			}
		}
	}
}
