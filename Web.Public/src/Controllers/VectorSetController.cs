using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.ClaimsVerifiers;
using Web.Public.Configs;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;
using Web.Public.Services.MessagePayloadValidators;

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
        private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
        private readonly VectorSetConfig _vectorSetConfig;

        public VectorSetController(IVectorSetService vectorSetService, 
            ITestSessionService testSessionService, 
            IJsonWriterService jsonWriter,
            IJsonReaderService jsonReader,
            IMessageService messageService,
            IJwtService jwtService,
            IMessagePayloadValidatorFactory workflowItemValidatorFactory,
            IOptions<VectorSetConfig> vectorSetConfig)
        {
            _vectorSetService = vectorSetService;
            _testSessionService = testSessionService;
            _jsonWriter = jsonWriter;
            _jsonReader = jsonReader;
            _messageService = messageService;
            _jwtService = jwtService;
            _workflowItemValidatorFactory = workflowItemValidatorFactory;
            _vectorSetConfig = vectorSetConfig.Value;
        }
        
        [HttpGet]
        public ActionResult GetVectorSets(long tsID)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new TestSessionClaimsVerifier(tsID);
            if (claimValidator.AreClaimsValid(claims))
            {            
                var testSessions = _testSessionService.GetTestSession(tsID);
            
                var vectorSetUrls = new VectorSetUrlObject
                {
                    VectorSetURLs = testSessions.VectorSetURLs
                };
            
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(vectorSetUrls));
            }

            return new ForbidResult();
        }

        [HttpGet("{vsID}")]
        public ActionResult GetPrompt(long tsID, long vsID)
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
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(prompt.Content));
            }

            return new ForbidResult();
        }

        [HttpDelete("{vsID}")]
        public ActionResult CancelVectorSet(long tsID, long vsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                // Convert and validate
                var payload = new CancelPayload {TestSessionID = tsID, VectorSetID = vsID};
                var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.CancelVectorSet).Validate(payload);
                if (!validation.IsSuccess)
                {
                    throw new JsonReaderException(validation.Errors);
                }
                
                // Pass to message queue
                var requestID = _messageService.InsertIntoQueue(APIAction.CancelVectorSet, cert, payload);

                // Build request object for response
                var requestObject = new RequestObject
                {
                    RequestID = requestID,
                    Status = RequestStatus.Initial
                };

                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
            }

            return new ForbidResult();
        }

        [HttpGet("{vsID}/results")]
        public ActionResult GetValidationResults(long tsID, long vsID)
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
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(validation.Content));
            }

            return new ForbidResult();
        }

        [HttpPost("{vsID}/results")]
        public ActionResult PostResults(long tsID, long vsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var submittedResults = _jsonReader.GetMessagePayloadFromBodyJson<VectorSetSubmissionPayload>(body, APIAction.SubmitVectorSetResults);

            // Validate registrations and return at that point if any failures occur.
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                // Convert and validate
                var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.SubmitVectorSetResults).Validate(submittedResults);
                if (!validation.IsSuccess)
                {
                    throw new JsonReaderException(validation.Errors);
                }
                
                _messageService.InsertIntoQueue(APIAction.SubmitVectorSetResults, cert, submittedResults);
                
                // TODO this is not the correct response to successful post of answers
                // TODO Though is what the spec says, it's not what we currently return :/
                return new AcceptedResult();
            }
            return new ForbidResult();
            
        }

        [HttpPut("{vsID}/results")]
        public ActionResult UpdateResults(long tsID, long vsID)
        {
            if (!_vectorSetConfig.AllowResubmission)
            {
                return new NotFoundResult();
            }
            
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            
            // TODO putting of results should only be allowed when the vector set is ready for them (specific status)?
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var submittedResults = _jsonReader.GetMessagePayloadFromBodyJson<VectorSetSubmissionPayload>(body, APIAction.ResubmitVectorSetResults);

            // Validate registrations and return at that point if any failures occur.
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                // Convert and validate
                var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.ResubmitVectorSetResults).Validate(submittedResults);
                if (!validation.IsSuccess)
                {
                    throw new JsonReaderException(validation.Errors);
                }
                
                _messageService.InsertIntoQueue(APIAction.ResubmitVectorSetResults, cert, submittedResults);
                
                // TODO this is likely not the correct response to return
                return new AcceptedResult();
            }

            return new ForbidResult();
        }

        [HttpGet("{vsID}/expected")]
        public ActionResult GetExpectedResults(long tsID, long vsID)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                // If the session isn't a sample, then the expected results are not generated
                var testSessions = _testSessionService.GetTestSession(tsID);
                if (!testSessions.IsSample)
                {
                    return new NotFoundResult();
                }
                
                var expectedResults = _vectorSetService.GetExpectedResults(vsID);
                if (expectedResults == null)
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }

                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(expectedResults.Content));
            }

            return new ForbidResult();
        }
    }
}