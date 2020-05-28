using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface IRequestService
    {
        Request GetRequest(long id);
        (long TotalCount, List<Request> Requests) GetPagedRequestsForUser(long userID, PagingOptions pagingOptions);
    }
}