using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using NIST.CVP.Libraries.Shared.Results;
using Serilog;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public class TestSessionProvider : ITestSessionProvider
    {
        private readonly string _connectionString;

        public TestSessionProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public bool IsOwner(long userID, long tsID)
        {
            var db = new MightyOrm(_connectionString);
            
            try
            {
                var result = db.ExecuteProcedure("acvp.TestSessionCheckOwner", new
                {
                    TestSessionID = tsID,
                    UserID = userID
                },
                new
                {
                    Result = false
                });

                return result.Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to check ownership");
                throw;
            }
        }

        public TestSession GetTestSession(long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.SingleFromProcedure("acvp.TestSessionGet", new
                {
                    ID = id
                });

                if (data == null)
                {
                    throw new Exception("Could not find test session");
                }
                
                var testSession = new TestSession
                {
                    ID = id,
                    CreatedOn = data.CreatedOn,
                    IsSample = data.Sample,
                    Passed = data.Disposition,
                    Publishable = data.Publishable,
                    Published = data.Published
                };

                var vsData = db.QueryFromProcedure("acvp.VectorSetGetFromTestSession", new
                {
                    TestSessionID = id
                });

                if (vsData == null)
                {
                    throw new Exception("Could not find vector sets");
                }

                testSession.VectorSetIDs = vsData.Select(vs => (long)vs.ID).ToList();

                return testSession;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error retrieving TestSession: {id}");
                throw;
            }
        }

        public List<TestSession> GetTestSessionList(long userID)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.QueryFromProcedure("acvp.TestSessionsGet", new
                {
                    UserID = userID
                });

                if (data == null)
                {
                    throw new Exception("Unable to get test sessions");
                }

                var testSessions = data.Select(tsID => new TestSession
                {
                    ID = tsID.ID,
                    CreatedOn = tsID.CreatedOn,
                    IsSample = tsID.Sample,
                    Passed = tsID.Disposition,
                    Publishable = tsID.Publishable
                }).ToList();

                foreach (var testSession in testSessions)
                {
                    var vsData = db.QueryFromProcedure("acvp.VectorSetGetFromTestSession", new
                    {
                        TestSessionID = testSession.ID
                    });

                    if (vsData == null)
                    {
                        throw new Exception($"Could not find vector sets for test session: {testSession.ID}");
                    }
                    
                    testSession.VectorSetIDs = vsData.Select(vs => (long)vs.ID).ToList();
                }

                return testSessions;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error retrieving TestSessions for user: {userID}");
                throw;
            }
        }

        public long GetNextTestSessionID()
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var nextID = db.SingleFromProcedure("external.TestSessionGetNextID");

                if (nextID == null)
                {
                    throw new Exception("Unable to get next ID");
                }

                return (long)nextID.ID;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving next TestSession ID");
                throw;
            }
        }

        public Result SetTestSessionPublished(long testSessionId)
        {
            var db = new MightyOrm(_connectionString);
        
            try
            {
                db.ExecuteProcedure("acvp.TestSessionSetPublished", new
                {
                    testSessionId
                });
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }
            
            return new Result();
        }
    }
}