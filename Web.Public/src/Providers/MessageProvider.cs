using System;
using ACVPWorkflow;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;
using Web.Public.Helpers;

namespace Web.Public.Providers
{
    public class MessageProvider : IMessageProvider
    {
        private readonly string _connectionString;
        
        public MessageProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }
        
        public void InsertIntoQueue(APIAction apiAction, long requestID, long userID, object content)
        {
            // Build json message to go into table
            var requestJson = JsonHelper.BuildRequestObject(requestID, apiAction, userID, content);

            throw new System.NotImplementedException();
        }

        public long GetNextRequestID()
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.SingleFromProcedure("external.RequestGetNextID");
                if (data == null)
                {
                    throw new Exception("Unable to get next request ID");
                }

                return data.ID;
            }
            catch (Exception ex)
            {
                Log.Error("Error getting next Request ID", ex);
                throw;
            }
        }
    }
}