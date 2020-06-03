using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.ClaimsVerifiers;
using Web.Public.Configs;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;
using Web.Public.Services.MessagePayloadValidators;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/testSessions/{tsID}/vectorSets")]
    public class VectorSetController : JwtAuthControllerBase
    {
        private readonly IVectorSetService _vectorSetService;
        private readonly ITestSessionService _testSessionService;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IJsonReaderService _jsonReader;
        private readonly IMessageService _messageService;
        private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
        private readonly VectorSetConfig _vectorSetConfig;

        public VectorSetController(
            IJwtService jwtService,
            IVectorSetService vectorSetService, 
            ITestSessionService testSessionService, 
            IJsonWriterService jsonWriter,
            IJsonReaderService jsonReader,
            IMessageService messageService,
            IMessagePayloadValidatorFactory workflowItemValidatorFactory,
            IOptions<VectorSetConfig> vectorSetConfig)
            : base (jwtService)
        {
            _vectorSetService = vectorSetService;
            _testSessionService = testSessionService;
            _jsonWriter = jsonWriter;
            _jsonReader = jsonReader;
            _messageService = messageService;
            _workflowItemValidatorFactory = workflowItemValidatorFactory;
            _vectorSetConfig = vectorSetConfig.Value;
        }
        
        [HttpGet]
        public ActionResult GetVectorSets(long tsID)
        {
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new TestSessionClaimsVerifier(tsID);
            if (claimValidator.AreClaimsValid(claims))
            {            
                var testSession = _testSessionService.GetTestSession(tsID);
            
                if (testSession == null)
                {
                    // This can be null in cases where the test session was POSTed but has not yet been replicated to public.
                    // Check that is not the case
                    if (_testSessionService.IsTestSessionQueued(tsID))
                    {
                        return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                    }
                    
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
                    {
                        Error = Request.HttpContext.Request.Path,
                        Context = $"Unable to find test session with id {tsID}."
                    }), HttpStatusCode.NotFound);
                }

                //Send a TestSessionKeepAlive message
                var payload = new TestSessionKeepAlivePayload { TestSessionID = tsID };
                _messageService.InsertIntoQueue(APIAction.TestSessionKeepAlive, GetCertSubjectFromJwt(), payload);

                var vectorSetUrls = new VectorSetUrlObject
                {
                    VectorSetURLs = testSession.VectorSetURLs
                };
            
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(vectorSetUrls));
            }

            return new ForbidResult();
        }

        [HttpGet("{vsID}")]
        public async Task<ActionResult> GetPrompt(long tsID, long vsID)
        {
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                //Send a TestSessionKeepAlive message
                var payload = new TestSessionKeepAlivePayload { TestSessionID = tsID };
                _messageService.InsertIntoQueue(APIAction.TestSessionKeepAlive, GetCertSubjectFromJwt(), payload);

                var prompt = await _vectorSetService.GetPromptAsync(vsID);
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
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                // Convert and validate
                var payload = new CancelPayload {TestSessionID = tsID, VectorSetID = vsID};
                var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.CancelVectorSet).Validate(payload);
                if (!validation.IsSuccess)
                {
                    throw new PayloadValidatorException(validation.Errors);
                }
                
                // Pass to message queue
                _messageService.InsertIntoQueue(APIAction.CancelVectorSet, GetCertSubjectFromJwt(), payload);

                // Build request object for response
                var requestObject = new CancelObject()
                {
                    Url = Request.Path.Value
                };

                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
            }

            return new ForbidResult();
        }

        [HttpGet("{vsID}/results")]
        public async Task<ActionResult> GetValidationResults(long tsID, long vsID)
        {
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                //Send a TestSessionKeepAlive message
                var payload = new TestSessionKeepAlivePayload { TestSessionID = tsID };
                _messageService.InsertIntoQueue(APIAction.TestSessionKeepAlive, GetCertSubjectFromJwt(), payload);

                // Short circuit, if answers were resubmitted the "/results" file will exist, we don't want to return it.
                if (_vectorSetService.GetStatus(vsID) == VectorSetStatus.ResubmitAnswers)
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }
                
                var validation = await _vectorSetService.GetValidationAsync(vsID);
                if (validation == null)
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(validation.Content));
            }

            return new ForbidResult();
        }

        [HttpPost("{vsID}/results")]
        public async Task<ActionResult> PostResults(long tsID, long vsID)
        {
            //Validate claim
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);

            if (claimValidator.AreClaimsValid(claims))
            {
                // Parse request
                var submittedResults = await _jsonReader.GetMessagePayloadFromBodyJsonAsync<VectorSetSubmissionPayload>(Request.Body, APIAction.SubmitVectorSetResults);

                // Convert and validate
                var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.SubmitVectorSetResults).Validate(submittedResults);
                if (!validation.IsSuccess)
                {
                    throw new PayloadValidatorException(validation.Errors);
                }
                
                _messageService.InsertIntoQueue(APIAction.SubmitVectorSetResults, GetCertSubjectFromJwt(), submittedResults);
                _vectorSetService.SetStatus(vsID, VectorSetStatus.KATReceived);

                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new VectorSetPostAnswersObject(tsID, vsID)));
            }

            return new ForbidResult();
            
        }

        [HttpPut("{vsID}/results")]
        public async Task<ActionResult> UpdateResults(long tsID, long vsID)
        {
            //Check that resubmissions are allowed in this environment
            if (!_vectorSetConfig.AllowResubmission)
            {
                return new NotFoundResult();
            }
            
            //Validate claim
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);

            if (claimValidator.AreClaimsValid(claims))
            {
                // Parse request
                var submittedResults = await _jsonReader.GetMessagePayloadFromBodyJsonAsync<VectorSetSubmissionPayload>(Request.Body, APIAction.ResubmitVectorSetResults);

                // Convert and validate
                var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.ResubmitVectorSetResults).Validate(submittedResults);
                if (!validation.IsSuccess)
                {
                    throw new PayloadValidatorException(validation.Errors);
                }
                
                _messageService.InsertIntoQueue(APIAction.ResubmitVectorSetResults, GetCertSubjectFromJwt(), submittedResults);
                _vectorSetService.SetStatus(vsID, VectorSetStatus.ResubmitAnswers);

                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new VectorSetPostAnswersObject(tsID, vsID)));
            }

            return new ForbidResult();
        }

        [HttpGet("{vsID}/expected")]
        public async Task<ActionResult> GetExpectedResults(long tsID, long vsID)
        {
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new VectorSetClaimsVerifier(tsID, vsID);
            if (claimValidator.AreClaimsValid(claims))
            {
                //Send a TestSessionKeepAlive message
                var payload = new TestSessionKeepAlivePayload { TestSessionID = tsID };
                _messageService.InsertIntoQueue(APIAction.TestSessionKeepAlive, GetCertSubjectFromJwt(), payload);

                // If the session isn't a sample, then the expected results are not generated
                var testSessions = _testSessionService.GetTestSession(tsID);
                if (!testSessions.IsSample)
                {
                    return new NotFoundResult();
                }
                
                var expectedResults = await _vectorSetService.GetExpectedResultsAsync(vsID);
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