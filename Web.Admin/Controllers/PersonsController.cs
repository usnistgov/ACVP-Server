using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.ACVPCore.Models;
using NIST.CVP.Libraries.Internal.ACVPCore.Models.Parameters;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.Results;

namespace Web.Admin.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PersonsController : ControllerBase
	{
		private readonly IPersonService _personService;

		public PersonsController(IPersonService personService)
		{
			_personService = personService;
		}

		[HttpGet("{personID}")]
		public Person Get(long personID)
		{
			return _personService.Get(personID);
		}

        [HttpPost]
        public PagedEnumerable<PersonLite> GetPersons(PersonListParameters param)
        {
            return _personService.Get(param);
        }

		[HttpPatch("{personID}")]
		public Result UpdatePerson(long personID, Person person)
		{
			PersonUpdateParameters parameters = new PersonUpdateParameters();
			parameters.PhoneNumbers = new List<(string Type, string Number)>();
			parameters.EmailAddresses = new List<string>();

			parameters.ID = personID;

			if (person.Name != null)
			{
				parameters.Name = person.Name;
				parameters.NameUpdated = true;
			}
			if (person.PhoneNumbers != null)
			{
				foreach (PersonPhone phone in person.PhoneNumbers)
				{
					parameters.PhoneNumbers.Add((phone.Type, phone.Number));
				}
				parameters.PhoneNumbersUpdated = true;
			}
			if (person.EmailAddresses != null)
			{
				foreach (string email in person.EmailAddresses)
				{
					parameters.EmailAddresses.Add(email);
				}
				parameters.EmailAddressesUpdated = true;
			}

			return _personService.Update(parameters);
		}
	}
}
