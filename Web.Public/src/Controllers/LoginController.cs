using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/[controller]")]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITotpService _totpService;
        private readonly IJwtService _jwtService;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IJsonReaderService _jsonReader;

        public LoginController(ITotpService totpService, IJwtService jwtService, IJsonWriterService jsonWriter, IJsonReaderService jsonReader)
        {
            _totpService = totpService;
            _jwtService = jwtService;
            _jsonWriter = jsonWriter;
            _jsonReader = jsonReader;
        }
        
        // TODO is it possible to separate the methods based on the body coming in?
        // PostPassword only includes TOTP password
        // RefreshToken includes both TOTP password AND the previous JWT

        [HttpPost]
        public JsonResult Login()
        {
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var content = _jsonReader.GetObjectFromBodyJson<JwtRequestObject>(body);
            
            // Grab user from authentication
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            
            // Validate TOTP
            var result = _totpService.ValidateTotp(cert, content.Password);

            // If no validation, don't proceed
            if (!result.IsSuccess)
            {
                var errorObject = new ErrorObject
                {
                    Error = $"Access denied! Reason: {result.ErrorMessage}"
                };
                
                return new JsonHttpStatusResult(
                    _jsonWriter.BuildVersionedObject(
                        errorObject),
                    HttpStatusCode.Forbidden);
            }
            
            // Either create or refresh the token
            var tokenResult = content.AccessToken == null ? _jwtService.Create() : _jwtService.Refresh(content.AccessToken);
            if (!tokenResult.IsSuccess)
            {
                var errorObject = new ErrorObject
                {
                    Error = tokenResult.ErrorMessage
                };
                
                return new JsonHttpStatusResult(
                    _jsonWriter.BuildVersionedObject(
                        errorObject), 
                    HttpStatusCode.Forbidden);
            }

            return new JsonHttpStatusResult(
                _jsonWriter.BuildVersionedObject(
                    new JwtObject
                    {
                        AccessToken = tokenResult.Token
                    }));
        }
    }
}