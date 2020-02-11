using System;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;

namespace Web.Public.Controllers
{
    [Route("acvp/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITotpProvider _totpProvider;
        
        public LoginController(ITotpProvider totpProvider)
        {
            _totpProvider = totpProvider;
        }
        
        // TODO is it possible to separate the methods based on the body coming in?
        // PostPassword only includes TOTP password
        // RefreshToken includes both TOTP password AND the previous JWT

        [HttpPost]
        public JsonResult Login(JwtRefreshObject content)
        {
            // Grab user from authentication
            
            // Grab password from body JSON
            
            // Validate Totp
            
            // Db verification? maybe included in Validate step
            
            // Build Access Token (JWT)
            throw new NotImplementedException();
        }
    }
}