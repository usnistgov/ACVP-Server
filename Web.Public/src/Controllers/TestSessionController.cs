using System.Linq;
using System.Net;
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
    [Route("acvp/v1/testSessions")]
    public class TestSessionController : JwtAuthControllerBase
    {
        private readonly ITestSessionService _testSessionService;
        private readonly IMessageService _messageService;
        private readonly IJsonReaderService _jsonReader;
        private readonly IJsonWriterService _jsonWriter;
        private readonly VectorSetConfig _vectorSetConfig;
        private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
        
        public TestSessionController(
            IJwtService jwtService,
            ITestSessionService testSessionService, 
            IMessageService messageService, 
            IJsonReaderService jsonReader,
            IJsonWriterService jsonWriter,
            IMessagePayloadValidatorFactory workflowItemValidatorFactory,
            IOptions<VectorSetConfig> vectorSetConfig)
            : base (jwtService)
        {
            _testSessionService = testSessionService;
            _messageService = messageService;
            _jsonReader = jsonReader;
            _jsonWriter = jsonWriter;
            _workflowItemValidatorFactory = workflowItemValidatorFactory;
            _vectorSetConfig = vectorSetConfig.Value;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetTestSessionsForUser()
        {
            //Try to read limit and offset, if passed in
            var limit = 0;
            var offset = 0;
            if (Request.Query.TryGetValue("limit", out var stringLimit))
            {
                int.TryParse(stringLimit.First(), out limit);
            }

            if (Request.Query.TryGetValue("offset", out var stringOffset))
            {
                int.TryParse(stringOffset.First(), out offset);
            }

            //If limit was not present, or a garbage value, make it a default
            if (limit <= 0) limit = 20;

            var pagingOptions = new PagingOptions
            {
                Limit = limit,
                Offset = offset
            };

            // Note this has permission to change Limit, if 20 is too big for data
            var (totalRecords, testSessions) = _testSessionService.GetTestSessionList(GetCertSubjectFromJwt(), pagingOptions);
            var pagedData =  new PagedResponse<TestSession>(totalRecords, testSessions, $"/acvp/v1/testSessions", pagingOptions);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));   
        }
        
        [HttpPost]
        public ActionResult CreateTestSession()
        {
            var certSubject = GetCertSubjectFromJwt();
            
            var apiAction = APIAction.RegisterTestSession;
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var registration = _jsonReader.GetMessagePayloadFromBodyJson<TestSessionRegisterPayload>(body, apiAction);
            
            if (registration.IsSample && !_vectorSetConfig.AllowIsSample)
            {
                return new NotFoundResult();
            }
            
            // Convert and validate
            var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(apiAction).Validate(registration);
            if (!validation.IsSuccess)
            {
                throw new JsonReaderException(validation.Errors);
            }

            // This modifies registration along the way
            var testSession = _testSessionService.CreateTestSession(certSubject, registration);

            // Insert into queue
            _messageService.InsertIntoQueue(apiAction, certSubject, registration);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSession));
        }

        [HttpGet("{id}")]
        public ActionResult GetTestSession(long id)
        {
            var jwt = GetJwt();
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            
            var claimValidator = new TestSessionClaimsVerifier(id);
            if (!claimValidator.AreClaimsValid(claims))
            {
                return new ForbidResult();
            }
            
            var testSession = _testSessionService.GetTestSession(id);
            if (testSession == null)
            {
                // This can be null in cases where the test session was POSTed but has not yet been replicated to public.
                // Check that is not the case
                if (_testSessionService.IsTestSessionQueued(id))
                {
                    return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new RetryObject()));
                }
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
                {
                    Error = Request.HttpContext.Request.Path,
                    Context = $"Unable to find test session with id {id}."
                }), HttpStatusCode.NotFound);
            }

            //Send a TestSessionKeepAlive message
            var payload = new TestSessionKeepAlivePayload { TestSessionID = id };
            _messageService.InsertIntoQueue(APIAction.TestSessionKeepAlive, GetCertSubjectFromJwt(), payload);

            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSession));
        }

        [HttpPut("{id}")]
        public ActionResult CertifyTestSession(long id)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);

            var claimValidator = new TestSessionClaimsVerifier(id);
            if (!claimValidator.AreClaimsValid(claims))
            {
                return new ForbidResult();
            }
            
            // Convert and validate
            var apiAction = APIAction.CertifyTestSession;
            var payload = _jsonReader.GetMessagePayloadFromBodyJson<CertifyTestSessionPayload>(jsonBlob, apiAction);
            payload.TestSessionID = id;
            var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(apiAction).Validate(payload);
            if (!validation.IsSuccess)
            {
                throw new JsonReaderException(validation.Errors);
            }

            var requestId = _messageService.InsertIntoQueue(apiAction, GetCertSubjectFromJwt(), payload);

            // Set to ensure the certify request only happens once per ts. This table isn't replicated downwards
            _testSessionService.SetTestSessionPublished(id);
            
            // Build request object for response
            var requestObject = new RequestObject
            {
                RequestID = requestId,
                Status = RequestStatus.Initial
            };
			
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
        }

        [HttpDelete("{id}")]
        public ActionResult CancelTestSession(long id)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new TestSessionClaimsVerifier(id);
            if (!claimValidator.AreClaimsValid(claims))
            {
                return new ForbidResult();
            }

            // Convert and validate
            var payload = new CancelPayload {TestSessionID = id};
            var validation = _workflowItemValidatorFactory.GetMessagePayloadValidator(APIAction.CancelTestSession).Validate(payload);
            if (!validation.IsSuccess)
            {
                throw new JsonReaderException(validation.Errors);
            }
            
            _messageService.InsertIntoQueue(APIAction.CancelTestSession, GetCertSubjectFromJwt(), payload);
            var requestObject = new CancelObject()
            {
                Url = Request.Path.Value
            };

            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
        }

        [HttpGet("{id}/results")]
        public ActionResult GetTestSessionResults(long id)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);

            var claimValidator = new TestSessionClaimsVerifier(id);
            if (!claimValidator.AreClaimsValid(claims))
            {
                return new ForbidResult();
            }

            var testSessionResults = _testSessionService.GetTestSessionResults(id);

            if (testSessionResults == null)
            {
                return new NotFoundResult();
            }
                
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSessionResults));
        }
    }
}