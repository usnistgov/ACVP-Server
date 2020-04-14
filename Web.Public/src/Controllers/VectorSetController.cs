using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public VectorSetController(IVectorSetService vectorSetService, 
            ITestSessionService testSessionService, 
            IJsonWriterService jsonWriter)
        {
            _vectorSetService = vectorSetService;
            _testSessionService = testSessionService;
            _jsonWriter = jsonWriter;
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
        public JsonHttpStatusResult GetPrompt(int tsID, int vsID)
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;

            if (_testSessionService.IsOwner(cert, tsID))
            {
                var prompt = _vectorSetService.GetPrompt(tsID, vsID);
            }
            else
            {
                return new JsonHttpStatusResult(null, HttpStatusCode.Forbidden);
            }
            
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public JsonHttpStatusResult CancelVectorSet(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}/results")]
        public JsonHttpStatusResult GetValidationResults(int id)
        {
            throw new NotImplementedException();
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

        [HttpGet("{id}/expected")]
        public JsonHttpStatusResult GetExpectedResults(int id)
        {
            throw new NotImplementedException();
        }
    }
}