using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class RequestProvider : IRequestProvider
    {
        private readonly ILogger<RequestProvider> _logger;
        private readonly string _connectionString;
        
        public RequestProvider(ILogger<RequestProvider> logger, IConnectionStringFactory connectionStringFactory)
        {
            _logger = logger;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public Request GetRequest(long id)
        {
            var db = new MightyOrm<Request>(_connectionString);

            try
            {
                var requestData = db.SingleFromProcedure("dbo.RequestGet", new
                {
                    RequestId = id
                });

                return requestData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting request");
                return null;
            }
        }

        public bool CheckRequestInitialized(long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.ExecuteProcedure("dbo.RequestExists",
                    new
                    {
                        RequestId = id
                    },
                    new
                    {
                        Exists = false
                    });

                return data.exists;
            }
            catch (Exception e)
            {
                _logger.LogError("Unable to validate existence of request.", e);
                return false;
            }
        }

        public (long TotalCount, List<Request> Requests) GetPagedRequestsForUser(long userID, long offset, long limit)
        {
            var db = new MightyOrm<Request>(_connectionString);

            try
            {
                var requestData = db.QueryWithExpando("dbo.RequestGetFromUser", new
                {
                    ACVPUserId = userID,
                    Offset = offset,
                    Limit = limit
                },
                new
                {
                   TotalRecords = (long)0 
                });

                if (requestData == null)
                {
                    return (0, new List<Request>());
                }

                return (requestData.ResultsExpando.TotalRecords, requestData.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting requests");
                throw;
            }
        }

        public long GetNextRequestID()
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var data = db.SingleFromProcedure("dbo.RequestGetNextID");
                if (data == null)
                {
                    throw new Exception("Unable to get next request ID");
                }

                return (long)data.RequestId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting next Request ID");
                throw;
            }
        }
    }
}