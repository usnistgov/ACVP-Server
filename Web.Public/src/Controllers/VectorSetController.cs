using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.ClaimsVerifiers;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/testSessions/{tsID}/vectorSets")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class VectorSetController : ControllerBase
    {
        private readonly IVectorSetService _vectorSetService;
        private readonly ITestSessionService _testSessionService;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IJsonReaderService _jsonReader;
        private readonly IJwtService _jwtService;

        public VectorSetController(IVectorSetService vectorSetService, 
            ITestSessionService testSessionService, 
            IJsonWriterService jsonWriter,
            IJsonReaderService jsonReader,
            IJwtService jwtService)
        {
            _vectorSetService = vectorSetService;
            _testSessionService = testSessionService;
            _jsonWriter = jsonWriter;
            _jsonReader = jsonReader;
            _jwtService = jwtService;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetVectorSets(int tsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var testSessions = _testSessionService.GetTestSession(cert, tsID);
            
            var vectorSetUrls = new VectorSetUrlObject
            {
                VectorSetURLs = testSessions.VectorSetURLs
            };
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(vectorSetUrls));
        }

        [HttpGet("{vsID}")]
        public ActionResult GetPrompt(int tsID, int vsID)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                var prompt = _vectorSetService.GetPrompt(vsID);
                if (prompt == null)
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }
                else
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(prompt.Content));
                }
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpDelete("{id}")]
        public JsonHttpStatusResult CancelVectorSet(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{vsID}/results")]
        public ActionResult GetValidationResults(int tsID, int vsID)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                var validation = _vectorSetService.GetValidation(vsID);
                if (validation == null)
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }
                else
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(validation.Content));
                }
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPost("{id}/results")]
        public JsonHttpStatusResult PostResults(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}/results")]
        public JsonHttpStatusResult UpdateResults(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{vsID}/expected")]
        public ActionResult GetExpectedResults(int tsID, int vsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            // If the session isn't a sample, then the expected results are not generated
            var testSessions = _testSessionService.GetTestSession(cert, tsID);
            if (!testSessions.IsSample)
            {
                return new NotFoundResult();
            }
            
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                var expectedResults = _vectorSetService.GetExpectedResults(vsID);
                if (expectedResults == null)
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }
                else
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(expectedResults.Content));
                }
            }
            else
            {
                return new ForbidResult();
            }
        }
    }
}