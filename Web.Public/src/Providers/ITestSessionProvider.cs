using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public interface ITestSessionProvider
    {
        TestSession GetTestSession(long userID, long id);
        List<TestSession> GetTestSessionsForUser(long userID);
        long GetNextTestSessionID();
    }
}