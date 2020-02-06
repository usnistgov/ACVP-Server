using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OtpNet;

namespace Web.Public.Controllers
{
    // TODO Excluded from Prod
    [Route("acvp/[controller]")]
    [ApiController]
    public class TotpController : ControllerBase
    {
        private readonly IProtocolVersionWrapper _versionWrapper;
        
        public TotpController(IProtocolVersionWrapper versionWrapper)
        {
            _versionWrapper = versionWrapper;
        }
        
        [HttpGet]
        public JArray GetTotp()
        {
            // Use authentication to identify user
            
            // Grab seed from keystore
            var seed = new byte[] {0, 1, 2, 3};
            
            var totp = new Totp(seed, 30, OtpHashMode.Sha256, 8);
            var password = totp.ComputeTotp(DateTime.Now);
            var passwordObject = new JObject {["password"] = password};
            return _versionWrapper.WrapJson(passwordObject);
        }
    }
}