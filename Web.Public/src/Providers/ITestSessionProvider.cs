using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface ITestSessionProvider
    {
        bool IsOwner(long userID, long tsID);
        TestSession GetTestSession(long id);
        bool IsTestSessionQueued(long id);
        List<TestSession> GetTestSessionList(long userID);
        long GetNextTestSessionID();
        Result SetTestSessionSubmittedForApproval(long testSessionId);
        DateTime GetLastTouched(long testSessionID);
    }
}