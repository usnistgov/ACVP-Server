using System;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Mighty;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Serilog;
using Web.Public.Services;
using Web.Public.Models;

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

        public void InsertIntoQueue(APIAction apiAction, long userID, object content)
        {
            throw new NotImplementedException();
        }

        public void InsertIntoQueue(APIAction apiAction, long requestID, long userID, object content)
        {
            // Build json message to go into table
            var requestJson = _jsonWriter.BuildRequestWorkflowObject(requestID, content);

            var db = new MightyOrm(_connectionString);

            try
            {
                db.SingleFromProcedure("common.MessageQueueInsert", new
                {
                    MessageType = apiAction,
                    userId = userID,
                    Payload = requestJson
                });
                Log.Information($"Added requestID: {requestID} to the message queue");
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to insert message into message queue: {requestJson}", ex);
                throw;
            }
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

                return (long)data.ID;
            }
            catch (Exception ex)
            {
                Log.Error("Error getting next Request ID", ex);
                throw;
            }
        }
    }
}