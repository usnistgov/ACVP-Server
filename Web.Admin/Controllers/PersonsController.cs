using ACVPCore.ExtensionMethods;
using ACVPCore.Services;
using Microsoft.AspNetCore.Mvc;
using ACVPCore.Models;
using System;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

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

        [HttpPost("{personID}/phones")]
        public Result UpdatePerson(Person phone)
        {
            PersonUpdateParameters parameters = new PersonUpdateParameters();

            

            return null;
        }
    }
}
