using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface IRequestProvider
    {
        Request GetRequest(long id);
        bool CheckRequestInitialized(long id);
        List<Request> GetAllRequestsForUser(long userID);
    }
}