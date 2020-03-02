using Microsoft.AspNetCore.Mvc;
using Web.Public.Providers;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/algorithms")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAlgorithmProvider _algorithmProvider;

        public AlgorithmController(IJwtService jwtService, IAlgorithmProvider algorithmProvider)
        {
            _jwtService = jwtService;
            _algorithmProvider = algorithmProvider;
        }

        [HttpGet]
        public JsonResult GetAlgorithmList()
        {
            // Authenticate
            
            // Retrieve and return listing
                // (cache listing?)
                var list = _algorithmProvider.GetAlgorithmList();
                
                
            return new JsonResult("content not found");
        }
    }
}