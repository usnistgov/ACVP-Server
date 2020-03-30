using System;
using ACVPWorkflow;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;
using Web.Public.Services;

namespace Web.Public.Providers
{
    public class MessageProvider : IMessageProvider
    {
        private readonly string _connectionString;
        private readonly IJsonWriterService _jsonWriter;
        
        public MessageProvider(IConnectionStringFactory connectionStringFactory, IJsonWriterService jsonWriter)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
            _jsonWriter = jsonWriter;
        }
        
        public void InsertIntoQueue(APIAction apiAction, long requestID, long userID, object content)
        {
            // Build json message to go into table
            var requestJson = _jsonWriter.BuildRequestObject(requestID, apiAction, userID, content);

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