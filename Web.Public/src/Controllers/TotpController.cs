using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    // TODO Excluded from Prod, only used by developers
    [Route("acvp/[controller]")]
    [ApiController]
    public class TotpController : ControllerBase
    {
        private readonly ITotpService _totpService;
        private readonly IJsonWriterService _jsonWriter;
        
        public TotpController(ITotpService totpService, IJsonWriterService jsonWriter)
        {
            _totpService = totpService;
            _jsonWriter = jsonWriter;
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
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(passwordObject));
        }
    }
}