using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Results;

namespace Web.Public.Controllers
{
    [Route("acvp/testSessions/{tsID}/vectorSets")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class VectorSetController : ControllerBase
    {
        [HttpGet]
        public JsonHttpStatusResult GetVectorSets(int tsID)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetPrompt(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public JsonHttpStatusResult CancelVectorSet(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/results")]
        public JsonHttpStatusResult GetValidationResults(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}/results")]
        public JsonHttpStatusResult PostResults(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/results")]
        public JsonHttpStatusResult UpdateResults(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/expected")]
        public JsonHttpStatusResult GetExpectedResults(int id)
        {
            throw new NotImplementedException();
        }
    }
}