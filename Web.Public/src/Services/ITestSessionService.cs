using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface ITestSessionService
    {
        bool IsOwner(string userCertSubject, long id);
        TestSession GetTestSession(long id);
        bool IsTestSessionQueued(long id);
        (long TotalRecords, List<TestSession> TestSessions) GetTestSessionList(string userCertSubject, PagingOptions pagingOptions);
        TestSession CreateTestSession(string userCertSubject, TestSessionRegisterPayload registration);
        Result SetTestSessionSubmittedForApproval(long testSessionId);
        TestSessionResults GetTestSessionResults(long id);
        DateTime GetLastTouched(long testSessionID);
    }
}