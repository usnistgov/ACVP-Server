using System.Collections.Generic;
using NIST.CVP.Results;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface ITestSessionProvider
    {
        bool IsOwner(long userID, long tsID);
        TestSession GetTestSession(long userID, long id);
        List<TestSession> GetTestSessionList(long userID);
        long GetNextTestSessionID();
        Result SetTestSessionPublished(long testSessionId);
    }
}