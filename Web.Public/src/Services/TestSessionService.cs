using System;
using System.Collections.Generic;
using System.Linq;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class TestSessionService : ITestSessionService
    {
        private readonly ITestSessionProvider _testSessionProvider;
        private readonly IVectorSetProvider _vectorSetProvider;
        private readonly IUserProvider _userProvider;

        public TestSessionService(ITestSessionProvider testSessionProvider, IVectorSetProvider vectorSetProvider, IUserProvider userProvider)
        {
            _testSessionProvider = testSessionProvider;
            _vectorSetProvider = vectorSetProvider;
            _userProvider = userProvider;
        }

        public TestSession GetTestSession(byte[] cert, long id)
        {
            var userID = _userProvider.GetUserIDFromCertificate(cert);
            return _testSessionProvider.GetTestSession(userID, id);
        }

        public List<TestSession> GetTestSessionsForUser(byte[] cert)
        {
            var userID = _userProvider.GetUserIDFromCertificate(cert);
            return _testSessionProvider.GetTestSessionsForUser(userID);
        }

        public TestSession CreateTestSession(TestSessionRegistration registration)
        {
            // Get an ID for TestSession
            registration.ID = _testSessionProvider.GetNextTestSessionID();
            
            // Get IDs for each VectorSet
            foreach (var algo in registration.Algorithms)
            {
                algo.IsSample = registration.IsSample;
                algo.VsID = _vectorSetProvider.GetNextVectorSetID(registration.ID, "");
            }

            return new TestSession
            {
                ID = registration.ID,
                IsSample = registration.IsSample,
                VectorSetIDs = registration.Algorithms.Select(vs => vs.VsID).ToList(),
                AccessToken = "",
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now,
                Passed = false,
                Publishable = false
            };
        }
    }
}