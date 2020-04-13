using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.Providers;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/requests")]
    [Authorize]
    [TypeFilter(typeof(ExceptionFilter))]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IUserProvider _userProvider;

        public RequestController(IRequestService requestService, IJsonWriterService jsonWriter, IUserProvider userProvider)
        {
            _requestService = requestService;
            _jsonWriter = jsonWriter;
            _userProvider = userProvider;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetAllRequests()
        {
            var cert = HttpContext.Connection.ClientCertificate.RawData;
            var userID = _userProvider.GetUserIDFromCertificate(cert);

            var requests = _requestService.GetAllRequestsForUser(userID);
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requests));
        }
        
        [HttpGet("{id}")]
        public JsonHttpStatusResult GetRequest(int id)
        {
            // TODO should these endpoints be concerned about userID? 

            var request = _requestService.GetRequest(id);
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(request));
        }
    }
}