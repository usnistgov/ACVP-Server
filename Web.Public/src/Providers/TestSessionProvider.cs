using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Results;
using Web.Public.Configs;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class TestSessionProvider : ITestSessionProvider
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly TestSessionConfig _testSessionConfig;

        public TestSessionProvider(ILogger<TestSessionProvider> logger, IOptions<TestSessionConfig> testSessionConfig, IConnectionStringFactory connectionStringFactory)
        {
            _logger = logger;
            _testSessionConfig = testSessionConfig.Value;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public bool IsOwner(long userID, long tsID)
        {
            var db = new MightyOrm(_connectionString);
            
            try
            {
                var result = db.ExecuteProcedure("dbo.TestSessionCheckOwner", new
                {
                    TestSessionId = tsID,
                    ACVPUserId = userID
                },
                new
                {
                    Result = false
                });

                return result.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to check ownership");
                throw;
            }
        }

        public TestSession GetTestSession(long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.SingleFromProcedure("dbo.TestSessionGet", new
                {
                    TestSessionId = id
                });

                if (data == null)
                {
                    return null;
                }

                var testSession = new TestSession
                {
                    ID = id,
                    CreatedOn = data.CreatedOn,
                    IsSample = data.IsSample,
                    Status = (TestSessionStatus)data.TestSessionStatusId,
                    LastTouched = (DateTime)data.LastTouched,
                    ExpiresOn = ((DateTime)data.LastTouched).AddDays(_testSessionConfig.TestSessionExpirationAgeInDays)     //This is pretty lame to set both of these, but expiration is based on configuration, and don't want to pass config into a model class
                };

                var vsData = db.QueryFromProcedure("dbo.VectorSetGetFromTestSession", new
                {
                    TestSessionId = id
                });

                if (vsData == null)
                {
                    return null;
                }

                testSession.VectorSetIDs = vsData.Select(vs => (long)vs.VectorSetId).ToList();

                return testSession;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestSession: {id}");
                return null;
            }
        }

        public bool IsTestSessionQueued(long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.ExecuteProcedure("dbo.ExternalTestSessionExists",
                    new
                    {
                        TestSessionId = id
                    },
                    new
                    {
                        Exists = false
                    });

                if (data == null)
                {
                    return false;
                }

                return data.Exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestSession exists status: {id}");
                return false;
            }
        }

        public List<TestSession> GetTestSessionList(long userID)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.QueryFromProcedure("dbo.TestSessionsGet", new
                {
                    UserID = userID
                });

                if (data == null)
                {
                    throw new Exception("Unable to get test sessions");
                }

                var testSessions = data.Select(ts => new TestSession
                {
                    ID = ts.TestSessionId,
                    CreatedOn = ts.CreatedOn,
                    IsSample = ts.IsSample,
                    Status = (TestSessionStatus)ts.TestSessionStatusId,
                    ExpiresOn = ((DateTime)ts.LastTouched).AddDays(_testSessionConfig.TestSessionExpirationAgeInDays)
                }).ToList();

                foreach (var testSession in testSessions)
                {
                    var vsData = db.QueryFromProcedure("dbo.VectorSetGetFromTestSession", new
                    {
                        TestSessionId = testSession.ID
                    });

                    if (vsData == null)
                    {
                        throw new Exception($"Could not find vector sets for test session: {testSession.ID}");
                    }
                    
                    testSession.VectorSetIDs = vsData.Select(vs => (long)vs.VectorSetId).ToList();
                }

                return testSessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving TestSessions for user: {userID}");
                throw;
            }
        }

        public long GetNextTestSessionID()
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var nextID = db.SingleFromProcedure("dbo.TestSessionGetNextID");

                if (nextID == null)
                {
                    throw new Exception("Unable to get next ID");
                }

                return (long)nextID.TestSessionId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving next TestSession ID");
                throw;
            }
        }

        public Result SetTestSessionSubmittedForApproval(long testSessionId)
        {
            var db = new MightyOrm(_connectionString);
        
            try
            {
                db.ExecuteProcedure("dbo.TestSessionSetSubmittedForApproval", new
                {
                    TestSessionId = testSessionId
                });
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            
            return new Result();
        }

        public DateTime GetLastTouched(long testSessionID)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                //Get the last touched date, but if the TS does not exist, or the LastTouched is somehow null (shouldn't be), return MinValue
                var value = db.ScalarFromProcedure("acvp.TestSessionGetLastTouched", new { TestSessionId = testSessionID });
                return (value == null || value == DBNull.Value) ? DateTime.MinValue : (DateTime)value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving LastTouched for Test Session  {testSessionID}");
                return DateTime.MinValue;
            }
        }
    }
}