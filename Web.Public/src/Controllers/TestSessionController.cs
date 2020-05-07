using System;
using System.Linq;
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
    [Route("acvp/v1/testSessions")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class TestSessionController : ControllerBase
    {
        private readonly ITestSessionService _testSessionService;
        private readonly IMessageService _messageService;
        private readonly IJsonReaderService _jsonReader;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IJwtService _jwtService;
        private readonly VectorSetConfig _vectorSetConfig;
        private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;
        
        public TestSessionController(
            ITestSessionService testSessionService, 
            IMessageService messageService, 
            IJsonReaderService jsonReader,
            IJsonWriterService jsonWriter,
            IJwtService jwtService,
            IMessagePayloadValidatorFactory workflowItemValidatorFactory,
            IOptions<VectorSetConfig> vectorSetConfig)
        {
            _testSessionService = testSessionService;
            _messageService = messageService;
            _jsonReader = jsonReader;
            _jsonWriter = jsonWriter;
            _jwtService = jwtService;
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
            
            var cert = HttpContext.Connection.ClientCertificate.RawData;

            // Note this has permission to change Limit, if 20 is too big for data
            var (totalRecords, testSessions) = _testSessionService.GetTestSessionList(cert, pagingOptions);
            var pagedData =  new PagedResponse<TestSession>(totalRecords, testSessions, $"/acvp/v1/testSessions", pagingOptions);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));   
        }
        
        [HttpPost]
        public ActionResult CreateTestSession()
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;

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
            var testSession = _testSessionService.CreateTestSession(cert, registration);

            // Insert into queue
            _messageService.InsertIntoQueue(apiAction, cert, registration);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSession));
        }

        [HttpGet("{id}")]
        public ActionResult GetTestSession(long id)
        {
            var jwt = Request.Headers["Authorization"];
            var claims = _jwtService.GetClaimsFromJwt(jwt);
            
            var claimValidator = new TestSessionClaimsVerifier(id);
            if (!claimValidator.AreClaimsValid(claims))
            {
                return new ForbidResult();
            }
            
            var testSession = _testSessionService.GetTestSession(id);
            if (testSession == null)
            {
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
                {
                    Error = Request.HttpContext.Request.Path,
                    Context = $"Unable to find test session with id {id}."
                }), HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSession));
        }

        [HttpPut("{id}")]
        public ActionResult CertifyTestSession(long id)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
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

            var requestId = _messageService.InsertIntoQueue(apiAction, cert, payload);

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
            var cert = HttpContext.Connection.ClientCertificate.RawData;

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
            
            var requestId = _messageService.InsertIntoQueue(APIAction.CancelTestSession, cert, payload);
            var requestObject = new RequestObject
            {
                RequestID = requestId,
                Status = RequestStatus.Initial
            };

            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject));
        }
    }
}