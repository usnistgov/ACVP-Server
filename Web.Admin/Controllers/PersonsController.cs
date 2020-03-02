using ACVPCore.ExtensionMethods;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using ACVPCore.Models;
using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using System.Collections.Generic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonsController(
           IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet("{personID}")]
        public Person Get(long personID)
        {
            return _personService.Get(personID);
        }

        [HttpGet]
        public WrappedEnumerable<PersonLite> GetPersons(long pageSize, long pageNumber)
        {
            // Set some defaults in case no values are provided
            if (pageSize == 0) { pageSize = 10; }
            if (pageNumber == 0) { pageNumber = 1; }

            return _personService.Get(pageSize, pageNumber).WrapEnumerable();
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
