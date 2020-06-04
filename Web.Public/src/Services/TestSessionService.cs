using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Configs;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
    public class TestSessionService : ITestSessionService
    {
        private readonly IAlgorithmService _algorithmService;
        private readonly ITestSessionProvider _testSessionProvider;
        private readonly IVectorSetProvider _vectorSetProvider;
        private readonly IUserProvider _userProvider;
        private readonly IJwtService _jwtService;
        private readonly TestSessionConfig _testSessionConfig;

        public TestSessionService(IAlgorithmService algorithmService, ITestSessionProvider testSessionProvider, IVectorSetProvider vectorSetProvider, IUserProvider userProvider, IJwtService jwtService, IOptions<TestSessionConfig> testSessionConfig)
        {
            _algorithmService = algorithmService;
            _testSessionProvider = testSessionProvider;
            _vectorSetProvider = vectorSetProvider;
            _userProvider = userProvider;
            _jwtService = jwtService;
            _testSessionConfig = testSessionConfig.Value;
        }

        public bool IsOwner(string userCertSubject, long id)
        {
            var userID = _userProvider.GetUserIDFromCertificateSubject(userCertSubject);
            return _testSessionProvider.IsOwner(userID, id);
        }

        public TestSession GetTestSession(long id) => _testSessionProvider.GetTestSession(id);

        public bool IsTestSessionQueued(long id) => _testSessionProvider.IsTestSessionQueued(id);

        public (long TotalRecords, List<TestSession> TestSessions) GetTestSessionList(string userCertSubject, PagingOptions pagingOptions)
        {
            var userID = _userProvider.GetUserIDFromCertificateSubject(userCertSubject);
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

        public TestSession CreateTestSession(string userCertSubject, TestSessionRegisterPayload registration)
        {
            // Get an ID for TestSession
            registration.ID = _testSessionProvider.GetNextTestSessionID();
            
            // Get IDs for each VectorSet
            foreach (var algo in registration.Algorithms)
            {
                algo.IsSample = registration.IsSample;
                algo.VsID = _vectorSetProvider.GetNextVectorSetID(registration.ID, "");
                algo.AlgorithmId = _algorithmService.GetAlgorithm(algo.Algorithm, algo.Mode, algo.Revision).AlgorithmId;
            }

            var vectorSetIds = registration.Algorithms.Select(vs => vs.VsID).ToList();

            var claims = new Dictionary<string, string>
            {
                {"tsId", JsonSerializer.Serialize(registration.ID)},
                {"vsId", JsonSerializer.Serialize(vectorSetIds)}
            };
            var jwt = _jwtService.Create(userCertSubject, claims);
            var testSessionJwt = jwt.Token;
            
            return new TestSession
            {
                ID = registration.ID,
                IsSample = registration.IsSample,
                VectorSetIDs = vectorSetIds,
                AccessToken = testSessionJwt,
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(_testSessionConfig.TestSessionExpirationAgeInDays),
                Status = TestSessionStatus.Unknown  //This is goofy, but there's no need for the status at this point anyway
            };
        }

        public Result SetTestSessionPublished(long testSessionId) => _testSessionProvider.SetTestSessionPublished(testSessionId);
        public TestSessionResults GetTestSessionResults(long id)
        {
            var testSession = GetTestSession(id);

            if (testSession == null)
            {
                return null;
            }

            var returnObject = new TestSessionResults()
            {
                Status = testSession.Status,
                Type = new List<VectorSetResultsForTestSession>()
            };
            
            foreach (var vsId in testSession.VectorSetIDs)
            {
                returnObject.Type.Add(new VectorSetResultsForTestSession(id, vsId, _vectorSetProvider.GetStatus(vsId)));
            }

            return returnObject;
        }

        public DateTime GetLastTouched(long testSessionID) => _testSessionProvider.GetLastTouched(testSessionID);
    }
}