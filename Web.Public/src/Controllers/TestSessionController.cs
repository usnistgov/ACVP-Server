using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public TestSessionController(
            IParameterValidatorService parameterValidatorService,
            ITestSessionService testSessionService, 
            IMessageService messageService, 
            IJsonReaderService jsonReader,
            IJsonWriterService jsonWriter)
        {
            _parameterValidatorService = parameterValidatorService;
            _testSessionService = testSessionService;
            _messageService = messageService;
            _jsonReader = jsonReader;
            _jsonWriter = jsonWriter;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetTestSessionsForUser()
        {
            // TODO this needs paging and more data returned from the SP
            
            
            var cert = HttpContext.Connection.ClientCertificate.RawData;

            var testSessions = _testSessionService.GetTestSessionsForUser(cert);
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(testSessions));   
        }
        
        [HttpPost]
        public JsonHttpStatusResult CreateTestSession()
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            
            // Parse registrations
            var body = _jsonReader.GetJsonFromBody(Request.Body);
            var registration = _jsonReader.GetObjectFromBodyJson<TestSessionRegistration>(body);

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
            var testSession = _testSessionService.CreateTestSession(registration);

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
        public JsonHttpStatusResult CertifyTestSession(int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public JsonHttpStatusResult CancelTestSession(int id)
        {
            throw new NotImplementedException();
        }
    }
}