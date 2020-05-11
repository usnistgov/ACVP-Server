using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    // TODO Excluded from Prod, only used by developers
    [Route("acvp/[controller]")]
    [TypeFilter(typeof(ExceptionFilter))]
    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
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
            var certSubject = Request.HttpContext.Connection.ClientCertificate.Subject;
            
            // Compute Totp
            var result = _totpService.GenerateTotp(certSubject);

            // Wrap and return to user
            var passwordObject = new PasswordObject
            {
                Totp = result
            };
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(passwordObject));
        }
    }
}