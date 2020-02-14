using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Providers;

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
            var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;
            
            // Compute Totp
            var result = _totpProvider.GenerateTotp(certRawData);

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