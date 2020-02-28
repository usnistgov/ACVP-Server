using Microsoft.AspNetCore.Mvc;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/algorithms")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AlgorithmController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpGet]
        public JsonResult GetAlgorithmList()
        {
            // Authenticate
            
            // Retrieve and return listing
                // (cache listing?)
            
            return new JsonResult("content not found");
        }
    }
}