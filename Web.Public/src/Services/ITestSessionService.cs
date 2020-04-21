using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface ITestSessionService
    {
        bool IsOwner(byte[] cert, long id);
        TestSession GetTestSession(byte[] cert, long id);
        (long TotalRecords, List<TestSession> TestSessions) GetTestSessionList(byte[] cert, PagingOptions pagingOptions);
        TestSession CreateTestSession(byte[] cert, TestSessionRegistration registration);
        Result ValidateTestSessionCertifyRequest(byte[] cert, TestSessionCertify certifyRequest, long testSessionId);
        Result SetTestSessionPublished(long testSessionId);
    }
}