using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface ITestSessionService
    {
        TestSession GetTestSession(byte[] cert, long id);
        List<TestSession> GetTestSessionsForUser(byte[] cert);
        TestSession CreateTestSession(TestSessionRegistration registration);
    }
}