using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/[controller]")]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
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
            var clientCertSubject = HttpContext.Connection.ClientCertificate.Subject;
            
            // Validate TOTP
            var result = _totpService.ValidateTotp(clientCertSubject, content.Password);

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
            var tokenResult = string.IsNullOrEmpty(content.AccessToken) ? _jwtService.Create(clientCertSubject, null) : _jwtService.Refresh(clientCertSubject, content.AccessToken);
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