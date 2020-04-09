using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Services
{
    public interface ITestSessionService
    {
        TestSession CreateTestSession(TestSessionRegistration registration);
    }
}