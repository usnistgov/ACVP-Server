using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;

namespace Web.Public.Controllers
{
    // TODO Excluded from Prod, only used by developers
    [Route("acvp/[controller]")]
    [ApiController]
    public class TotpController : ControllerBase
    {
        private readonly ITotpProvider _totpProvider;
        
        public TotpController(ITotpProvider totpProvider)
        {
            _totpProvider = totpProvider;
        }
        
        [HttpGet]
        public JsonResult GetTotp()
        {
            // Use authentication to identify user
            var x = Request.Headers.Keys;
            var y = Request.Headers.Values;

            var cert = Request.HttpContext.Connection.ClientCertificate;
            
            // Grab seed from keystore
            var seed = new byte[] {0, 1, 2, 3};
            
            // Compute Totp
            var result = _totpProvider.GenerateTotp(seed);

            // Wrap and return to user
            var returnObject = new PasswordObject
            {
                AcvVersion = "1.0",
                Password = result
            };

            return new JsonResult(returnObject);
        }
    }
}