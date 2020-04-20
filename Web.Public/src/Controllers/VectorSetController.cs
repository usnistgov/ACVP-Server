using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Public.ClaimsVerifiers;
using Web.Public.Configs;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
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
        private readonly IMessageService _messageService;
        private readonly IJwtService _jwtService;
        private readonly VectorSetConfig _vectorSetConfig;

        public VectorSetController(IVectorSetService vectorSetService, 
            ITestSessionService testSessionService, 
            IJsonWriterService jsonWriter,
            IJsonReaderService jsonReader,
            IMessageService messageService,
            IJwtService jwtService,
            IOptions<VectorSetConfig> vectorSetConfig)
        {
            _vectorSetService = vectorSetService;
            _testSessionService = testSessionService;
            _jsonWriter = jsonWriter;
            _jsonReader = jsonReader;
            _messageService = messageService;
            _jwtService = jwtService;
            _vectorSetConfig = vectorSetConfig.Value;
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

        [HttpDelete("{vsID}")]
        public ActionResult CancelVectorSet(int tsID, int vsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;

            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                // Pass to message queue
                var requestID = _messageService.InsertIntoQueue(APIAction.CancelVectorSet, cert, null);

                // Build request object for response
                var requestObject = new RequestObject
                {
                    RequestID = requestID,
                    Status = RequestStatus.Initial
                };

                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
            }
            else
            {
                return new ForbidResult();
            }
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

        [HttpPost("{vsID}/results")]
        public ActionResult PostResults(int tsID, int vsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var submittedResults = _jsonReader.GetObjectFromBodyJson<VectorSetResult>(body);

            // Validate registrations and return at that point if any failures occur.
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                _messageService.InsertIntoQueue(APIAction.SubmitVectorSetResults, cert, submittedResults);
                return new AcceptedResult();
            }
            else
            {
                return new ForbidResult();
            }
        }

        [HttpPut("{vsID}/results")]
        public ActionResult UpdateResults(int tsID, int vsID)
        {
            if (!_vectorSetConfig.AllowResubmission)
            {
                return new NotFoundResult();
            }
            
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var submittedResults = _jsonReader.GetObjectFromBodyJson<VectorSetResult>(body);

            // Validate registrations and return at that point if any failures occur.
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                _messageService.InsertIntoQueue(APIAction.ResubmitVectorSetResults, cert, submittedResults);
                return new AcceptedResult();
            }
            else
            {
                return new ForbidResult();
            }
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