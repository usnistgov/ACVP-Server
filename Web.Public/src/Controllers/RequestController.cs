using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Web.Public.JsonObjects;
using Web.Public.Models;
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
        public JsonHttpStatusResult GetPagedRequestsForUser()
        {
            var userID = _userProvider.GetUserIDFromCertificateSubject(GetCertSubjectFromJwt());

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

            //Build the querystring that excluded limit and offset
            var filterString = string.Join("&", Request.Query.Where(x => x.Key != "limit" && x.Key != "offset").Select(x => $"{x.Key}={x.Value.FirstOrDefault()}"));
            
            var pagingOptions = new PagingOptions
            {
                Limit = limit,
                Offset = offset
            };
            
            var data = _requestService.GetPagedRequestsForUser(userID, pagingOptions);
            var pagedData =  new PagedResponse<Request>(data.TotalCount, data.Requests, $"/acvp/v1/requests", pagingOptions);
				
            return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));
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