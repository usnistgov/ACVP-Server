using Microsoft.AspNetCore.Mvc;
using Web.Public.Helpers;
using Web.Public.JsonObjects;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    // TODO Excluded from Prod, only used by developers
    [Route("acvp/[controller]")]
    [ApiController]
    public class TotpController : ControllerBase
    {
        private readonly ITotpService _totpService;
        
        public TotpController(ITotpService totpService)
        {
            _totpService = totpService;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetTotp()
        {
            // Use authentication to identify user
            var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;
            
            // Compute Totp
            var result = _totpService.GenerateTotp(certRawData);

            // Wrap and return to user
            var passwordObject = new PasswordObject
            {
                Password = result
            };
            
            return new JsonHttpStatusResult(JsonHelper.BuildVersionedObject(passwordObject));
        }
    }
}