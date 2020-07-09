using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IRequestProvider
    {
        Request GetRequest(long id);
        bool CheckRequestInitialized(long id);
        (long TotalCount, List<Request> Requests) GetPagedRequestsForUser(long userID, long offset, long limit);
        long GetNextRequestID();
    }
}