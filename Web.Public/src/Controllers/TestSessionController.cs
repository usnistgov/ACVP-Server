using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/testSessions")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class TestSessionController : ControllerBase
    {
        private readonly ITestSessionService _testSessionService;
        private readonly IMessageService _messageService;
        private readonly IJsonReaderService _jsonReader;
        private readonly IJsonWriterService _jsonWriter;

        public TestSessionController(
            ITestSessionService testSessionService, 
            IMessageService messageService, 
            IJsonReaderService jsonReader,
            IJsonWriterService jsonWriter)
        {
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