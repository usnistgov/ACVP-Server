using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Results;

namespace Web.Public.Controllers
{
    [Route("acvp/testSessions")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class TestSessionController : ControllerBase
    {
        [HttpGet]
        public JsonHttpStatusResult GetTestSessionsForUser()
        {
            throw new NotImplementedException();   
        }
        
        [HttpPost]
        public JsonHttpStatusResult CreateTestSession()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetTestSession(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public JsonHttpStatusResult CertifyTestSession(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public JsonHttpStatusResult CancelTestSession(int id)
        {
            throw new NotImplementedException();
        }
    }
}