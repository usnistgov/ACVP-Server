using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITotpService _totpService;
        private readonly IJwtService _jwtService;
        
        public LoginController(ITotpService totpService, IJwtService jwtService)
        {
            _totpService = totpService;
            _jwtService = jwtService;
        }
        
        // TODO is it possible to separate the methods based on the body coming in?
        // PostPassword only includes TOTP password
        // RefreshToken includes both TOTP password AND the previous JWT

        [HttpPost]
        public JsonResult Login(JwtRequestObject content)
        {
            // Grab user from authentication
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            
            // Validate TOTP
            var result = _totpService.ValidateTotp(cert, content.Password);

            // If no validation, don't proceed
            if (!result.IsSuccess)
            {
                return new JsonResult($@"Access denied! D: Reason: {result.ErrorMessage}");
            }
            
            // Either create or refresh the token
            var tokenResult = content.AccessToken == null ? _jwtService.Create() : _jwtService.Refresh(content.AccessToken);
            if (!tokenResult.IsSuccess)
            {
                return new JsonResult(tokenResult.ErrorMessage);
            }
            
            return new JsonResult(new JwtObject
            {
                AcvVersion = "1.0",
                AccessToken = tokenResult.Token
            });
        }
    }
}