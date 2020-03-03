using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Providers;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/algorithms")]
    [Authorize]
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
            // Authenticate and refresh token
            
            // Retrieve and return listing
            var list = _algorithmProvider.GetAlgorithmList();
            
            return new JsonResult(new AlgorithmListObject {Jwt = new JwtObject(), AlgorithmList = list});
        }
    }
}