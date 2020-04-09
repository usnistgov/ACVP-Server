using System;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;

namespace Web.Public.Providers
{
    public class TestSessionProvider : ITestSessionProvider
    {
        private readonly string _connectionString;

        public TestSessionProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
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