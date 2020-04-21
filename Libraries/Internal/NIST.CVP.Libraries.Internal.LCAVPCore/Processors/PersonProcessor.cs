using System.Linq;
using NIST.CVP.Libraries.Shared.Results;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Processors
{
	public class PersonProcessor : IPersonProcessor
	{
		private readonly IPersonService _personService;

		public PersonProcessor(IPersonService personService)
		{
			_personService = personService;
		}

		public InsertResult Create(Contact person)
		{
			PersonCreateParameters createParameters = new PersonCreateParameters
			{
				Name = person.Name,
				OrganizationID = (long)person.OrganizationID,
				EmailAddresses = person.Emails,
				PhoneNumbers = person.PhoneNumbers.Where(x => !string.IsNullOrEmpty(x.Number)).Select(x => (x.Type, x.Number)).ToList()
			};

			return _personService.Create(createParameters);
		}

		public void Update(Contact person)
		{
			PersonUpdateParameters updateParameters = new PersonUpdateParameters
			{
				ID = person.PersonID,
			};

			//Name is only populated by LCAVP if updated. It will use an empty string if they want to wipe out the name (db does not allot null)
			if (person.Name != null)
			{
				updateParameters.Name = person.Name;
				updateParameters.NameUpdated = true;
			}

			if (person.Emails.Count > 0)
			{
				updateParameters.EmailAddresses = person.Emails;
				updateParameters.EmailAddressesUpdated = true;
			}

			if (person.PhoneNumbersUpdated)
			{
				//There's at least some change in the phone numbers, though it could be deleting 1 or more. Whatever is passed in is the new set, but need to filter out ones with empty numbers - that meant delete in the old world
				updateParameters.PhoneNumbersUpdated = true;
				updateParameters.PhoneNumbers = person.PhoneNumbers.Where(x => !string.IsNullOrEmpty(x.Number)).Select(x => (x.Type, x.Number)).ToList();
			}

			_personService.Update(updateParameters);
		}
	}
}
