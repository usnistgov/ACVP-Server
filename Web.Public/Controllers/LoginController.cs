using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Providers;

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
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            
            // Validate TOTP
            var result = _totpProvider.ValidateTotp(cert, content.Password);

            if (!result.IsSuccess)
            {
                return new JsonResult("Access denied! D:");
            }
            
            if (content.AccessToken != default)
            {
                // Is the old token used at all to construct the new one? 
                // TOTP password is always included for refresh... 
            }
            
            // Build Access Token (JWT)
            return new JsonResult(result);
        }
    }
}