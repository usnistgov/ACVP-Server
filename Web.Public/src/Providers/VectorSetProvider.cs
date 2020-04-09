using System;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;

namespace Web.Public.Providers
{
    public class VectorSetProvider : IVectorSetProvider
    {
        private readonly string _connectionString;
        
        public VectorSetProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public long GetNextVectorSetID(long tsID, string token)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var nextID = db.SingleFromProcedure("external.VectorSetGetNextID", new
                {
                    TestSessionID = tsID,
                    Token = token
                });

                if (nextID == null)
                {
                    throw new Exception("Unable to get next ID");
                }

                return (long)nextID.ID;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving next VectorSet ID");
                throw;
            }
        }
    }
}