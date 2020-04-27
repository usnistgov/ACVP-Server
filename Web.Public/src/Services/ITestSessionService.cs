using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface ITestSessionService
    {
        bool IsOwner(byte[] cert, long id);
        TestSession GetTestSession(long id);
        (long TotalRecords, List<TestSession> TestSessions) GetTestSessionList(byte[] cert, PagingOptions pagingOptions);
        TestSession CreateTestSession(byte[] cert, TestSessionRegisterPayload registration);
        Result SetTestSessionPublished(long testSessionId);
    }
}