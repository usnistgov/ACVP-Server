using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Serilog;
using Web.Public.Services;

namespace Web.Public.Providers
{
	public class MessageProvider : IMessageProvider
    {
        private readonly ILogger<MessageProvider> _logger;
        private readonly string _connectionString;
        private readonly IJsonWriterService _jsonWriter;
        
        public MessageProvider(ILogger<MessageProvider> logger, IConnectionStringFactory connectionStringFactory, IJsonWriterService jsonWriter)
        {
            _logger = logger;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
            _jsonWriter = jsonWriter;
        }

        public async Task InsertIntoQueueAsync(APIAction apiAction, long userID, object content)
        {
            // Build json message to go into table
            var json = _jsonWriter.BuildMessageObject(content);

            var db = new MightyOrm(_connectionString);

            try
            {
                await db.ExecuteProcedureAsync("common.MessageQueueInsert", new
                {
                    MessageType = apiAction,
                    userId = userID,
                    Payload = json
                }, commandTimeout: 120);
                Log.Information($"Added message to the message queue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert message into message queue: {json}");
                throw;
            }
        }

        public async Task InsertIntoQueueAsync(APIAction apiAction, long requestID, long userID, object content)
        {
            // Build json message to go into table
            var requestJson = _jsonWriter.BuildRequestWorkflowObject(requestID, content);

            var db = new MightyOrm(_connectionString);

            try
            {
                await db.ExecuteProcedureAsync("common.MessageQueueInsert", new
                {
                    MessageType = apiAction,
                    userId = userID,
                    Payload = requestJson
                }, commandTimeout: 120);
                Log.Information($"Added requestID: {requestID} to the message queue");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to insert message into message queue: {requestJson}");
                throw;
            }
        }
    }
}