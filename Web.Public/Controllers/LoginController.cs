using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Web.Public.Controllers
{
    [Route("acvp/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IProtocolVersionWrapper _versionWrapper;
        private readonly ITotpProvider _totpProvider;
        
        public LoginController(IProtocolVersionWrapper versionWrapper, ITotpProvider totpProvider)
        {
            _versionWrapper = versionWrapper;
            _totpProvider = totpProvider;
        }
        
        // TODO is it possible to separate the methods based on the body coming in?
        // PostPassword only includes TOTP password
        // RefreshToken includes both TOTP password AND the previous JWT

        [HttpPost]
        public JArray PostPassword(string password)
        {
            // Grab user from authentication
            
            // Grab password from body JSON
            
            // Validate Totp
            
            // Db verification? maybe included in Validate step
            
            // Build Access Token (JWT)
            throw new NotImplementedException();
        }

        [HttpPost]
        public JArray RefreshToken(string password, string oldJwt)
        {
            throw new NotImplementedException();
        }
    }
}