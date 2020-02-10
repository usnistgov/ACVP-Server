using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Web.Public.Controllers
{
    // TODO Excluded from Prod, only used by developers
    [Route("acvp/[controller]")]
    [ApiController]
    public class TotpController : ControllerBase
    {
        private readonly IProtocolVersionWrapper _versionWrapper;
        private readonly ITotpProvider _totpProvider;
        
        public TotpController(IProtocolVersionWrapper versionWrapper, ITotpProvider totpProvider)
        {
            _versionWrapper = versionWrapper;
            _totpProvider = totpProvider;
        }
        
        [HttpGet]
        public string GetTotp()
        {
            // Use authentication to identify user
            
            // Grab seed from keystore
            var seed = new byte[] {0, 1, 2, 3};
            
            // Compute Totp
            var result = _totpProvider.GenerateTotp(seed);
            var passwordObject = new JObject{["password"] = result};

            // Wrap and return to user
            return _versionWrapper.WrapJson(passwordObject);
        }
    }
}