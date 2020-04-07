using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/requests")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IJsonWriterService _jsonWriter;

        public RequestController(IRequestService requestService, IJsonWriterService jsonWriter)
        {
            _requestService = requestService;
            _jsonWriter = jsonWriter;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetAllRequests()
        {
            // TODO need paging, searching here
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public JsonHttpStatusResult GetRequest(int id)
        {
            var request = _requestService.GetRequest(id);
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(request));
        }
    }
}