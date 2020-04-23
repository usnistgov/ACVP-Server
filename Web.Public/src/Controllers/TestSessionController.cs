using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.ClaimsVerifiers;
using Web.Public.Configs;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/testSessions")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class TestSessionController : ControllerBase
    {
        private readonly IParameterValidatorService _parameterValidatorService;
        private readonly ITestSessionService _testSessionService;
        private readonly IMessageService _messageService;
        private readonly IJsonReaderService _jsonReader;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IJwtService _jwtService;
        private readonly VectorSetConfig _vectorSetConfig;
        
        public TestSessionController(
            IParameterValidatorService parameterValidatorService,
            ITestSessionService testSessionService, 
            IMessageService messageService, 
            IJsonReaderService jsonReader,
            IJsonWriterService jsonWriter,
            IJwtService jwtService,
            IOptions<VectorSetConfig> vectorSetConfig)
        {
            _parameterValidatorService = parameterValidatorService;
            _testSessionService = testSessionService;
            _messageService = messageService;
            _jsonReader = jsonReader;
            _jsonWriter = jsonWriter;
            _jwtService = jwtService;
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
        public JsonHttpStatusResult CreateTestSession()
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var registration = _jsonReader.GetObjectFromBodyJson<TestSessionRegistration>(body);

            if (registration.IsSample && !_vectorSetConfig.AllowIsSample)
            {
                var errorObject = new ErrorObject
                {
                    Error = "IsSample not supported"
                };
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(errorObject), HttpStatusCode.BadRequest);
            }
            
            // Validate registrations and return at that point if any failures occur.
            var parameterValidateResult = _parameterValidatorService.Validate(registration);
            if (!parameterValidateResult.IsSuccess)
            {
                var errorObject = new ErrorObject
                {
                    Error = $"One or more errors encountered when validating capabilities.",
                    Context = parameterValidateResult.ValidationErrors
                };
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(errorObject), HttpStatusCode.BadRequest);
            }
            
            // This modifies registration along the way
            var testSession = _testSessionService.CreateTestSession(cert, registration);

            // Insert into queue
            _messageService.InsertIntoQueue(APIAction.RegisterTestSession, cert, registration);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSession));
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetTestSession(int id)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;

            var testSession = _testSessionService.GetTestSession(cert, id);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSession));
        }

        [HttpPut("{id}")]
        public JsonHttpStatusResult CertifyTestSession(long id)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            
            var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);

            var testSessionCertifyRequest = _jsonReader.GetWorkflowObjectFromBodyJson<CertifyTestSessionPayload>(jsonBlob, APIAction.CertifyTestSession);
            testSessionCertifyRequest.TestSessionID = id;
            
            var certifiable = _testSessionService.ValidateTestSessionCertifyRequest(cert, testSessionCertifyRequest, id);
            if (!certifiable.IsSuccess)
            {
                var errorObject = new ErrorObject()
                {
                    Error = certifiable.ErrorMessage
                };
                
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(errorObject), HttpStatusCode.BadRequest);
            }
            
            var requestId = _messageService.InsertIntoQueue(APIAction.CertifyTestSession, cert, testSessionCertifyRequest);

            _testSessionService.SetTestSessionPublished(id);
            
            // Build request object for response
            var requestObject = new RequestObject
            {
                RequestID = requestId,
                Status = RequestStatus.Initial
            };
			
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
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

            var requestId = _messageService.InsertIntoQueue(APIAction.CancelTestSession, cert, new TestSessionCancel()
            {
                TestSessionId = id
            });
            
            var requestObject = new RequestObject
            {
                RequestID = requestId,
                Status = RequestStatus.Initial
            };

            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
        }
    }
}