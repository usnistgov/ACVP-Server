using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Providers;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
    [Route("acvp/v1/requests")]
    public class RequestController : JwtAuthControllerBase
    {
        private readonly IRequestService _requestService;
        private readonly IJsonWriterService _jsonWriter;
        private readonly IUserProvider _userProvider;

        public RequestController(
            IJwtService jwtService, 
            IRequestService requestService, 
            IJsonWriterService jsonWriter, 
            IUserProvider userProvider)
            : base (jwtService)
        {
            _requestService = requestService;
            _jsonWriter = jsonWriter;
            _userProvider = userProvider;
        }
        
        [HttpGet]
        public JsonHttpStatusResult GetAllRequests()
        {
            var userID = _userProvider.GetUserIDFromCertificateSubject(GetCertSubjectFromJwt());

            var requests = _requestService.GetAllRequestsForUser(userID);
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requests));
        }
        
        [HttpGet("{id}")]
        public JsonHttpStatusResult GetRequest(long id)
        {
            // TODO should these endpoints be concerned about userID? 

            var request = _requestService.GetRequest(id);
            if (request == null)
            {
                return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(new ErrorObject()
                {
                    Error = Request.HttpContext.Request.Path,
                    Context = $"Unable to find request with id {id}."
                }), HttpStatusCode.NotFound);
            }
            
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(request));
        }
    }
}