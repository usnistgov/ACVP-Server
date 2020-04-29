using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class TestSessionService : ITestSessionService
    {
        private readonly ITestSessionProvider _testSessionProvider;
        private readonly IVectorSetProvider _vectorSetProvider;
        private readonly IUserProvider _userProvider;
        private readonly IJwtService _jwtService;

        public TestSessionService(ITestSessionProvider testSessionProvider, IVectorSetProvider vectorSetProvider, IUserProvider userProvider, IJwtService jwtService)
        {
            _testSessionProvider = testSessionProvider;
            _vectorSetProvider = vectorSetProvider;
            _userProvider = userProvider;
            _jwtService = jwtService;
        }

        public bool IsOwner(byte[] cert, long id)
        {
            var userID = _userProvider.GetUserIDFromCertificate(cert);
            return _testSessionProvider.IsOwner(userID, id);
        }

        public TestSession GetTestSession(long id) => _testSessionProvider.GetTestSession(id);
        
        public (long TotalRecords, List<TestSession> TestSessions) GetTestSessionList(byte[] cert, PagingOptions pagingOptions)
        {
            var userID = _userProvider.GetUserIDFromCertificate(cert);
            var testSessionList = _testSessionProvider.GetTestSessionList(userID);

            // If there isn't enough for a full page, return the list
            if (testSessionList.Count <= pagingOptions.Limit)
            {
                pagingOptions.Limit = testSessionList.Count;
                return (testSessionList.Count, testSessionList);
            }

            // Fix limit so it doesn't go past the end of the list
            if (pagingOptions.Offset + pagingOptions.Limit > testSessionList.Count)
            {
                pagingOptions.Limit = testSessionList.Count - pagingOptions.Offset;
            }
            
            return (testSessionList.Count, testSessionList.GetRange(pagingOptions.Offset, pagingOptions.Limit));
        }

        public TestSession CreateTestSession(byte[] cert, TestSessionRegisterPayload registration)
        {
            // Get an ID for TestSession
            registration.ID = _testSessionProvider.GetNextTestSessionID();
            
            // Get IDs for each VectorSet
            foreach (var algo in registration.Algorithms)
            {
                algo.IsSample = registration.IsSample;
                algo.VsID = _vectorSetProvider.GetNextVectorSetID(registration.ID, "");
            }

            var vectorSetIds = registration.Algorithms.Select(vs => vs.VsID).ToList();

            var claims = new Dictionary<string, string>
            {
                {"tsId", JsonSerializer.Serialize(registration.ID)},
                {"vsId", JsonSerializer.Serialize(vectorSetIds)}
            };
            var jwt = _jwtService.Create(new X509Certificate2(cert).Subject, claims);
            var testSessionJwt = jwt.Token;
            
            return new TestSession
            {
                ID = registration.ID,
                IsSample = registration.IsSample,
                VectorSetIDs = vectorSetIds,
                AccessToken = testSessionJwt,
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now,
                Passed = false,
                Publishable = false
            };
        }

        public Result SetTestSessionPublished(long testSessionId) => _testSessionProvider.SetTestSessionPublished(testSessionId);
    }
}