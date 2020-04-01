using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;

namespace Web.Public.Controllers
{
    [Route("acvp/requests")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpGet]
        public void GetAllRequests()
        {
            
        }

        [HttpGet("{id}")]
        public void GetRequest(int id)
        {
            
        }
    }
}