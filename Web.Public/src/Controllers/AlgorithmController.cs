using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
            var jwt = HttpContext.GetTokenAsync("access_token").Result;
            var refreshedToken = _jwtService.Refresh(jwt);

            // Retrieve and return listing
            var list = _algorithmProvider.GetAlgorithmList();
            
            return new JsonResult(
                new AlgorithmListObject
                {
                    Jwt = new JwtObject
                    {
                        AccessToken = refreshedToken.IsSuccess ? refreshedToken.Token : "", 
                        AcvVersion = "1.0"
                    }, 
                    AlgorithmList = list
                });
        }
    }
}