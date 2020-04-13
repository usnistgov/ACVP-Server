using System;
using System.Collections.Generic;
using System.Linq;
using CVP.DatabaseInterface;
using Mighty;
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

        public TestSession GetTestSession(long userID, long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.SingleFromProcedure("acvp.TestSessionGet", new
                {
                    UserID = userID,
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
                    Publishable = data.Publishable
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
                Log.Error(ex, $"Error retrieving TestSession: {id} for user: {userID}");
                throw;
            }
        }

        public List<TestSession> GetTestSessionsForUser(long userID)
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

                return data.Select(tsID => new TestSession {ID = tsID.ID}).ToList();
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
    }
}