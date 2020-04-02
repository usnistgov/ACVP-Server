using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Results;

namespace Web.Public.Controllers
{
    [Route("acvp/requests")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpGet]
        public JsonHttpStatusResult GetAllRequests()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetRequest(int id)
        {
            throw new NotImplementedException();
        }
    }
}