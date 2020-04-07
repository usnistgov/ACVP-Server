using System;
using CVP.DatabaseInterface;
using Mighty;
using Serilog;
using Web.Public.Models;

namespace Web.Public.Providers
{
    public class RequestProvider : IRequestProvider
    {
        private readonly string _connectionString;
        
        public RequestProvider(IConnectionStringFactory connectionStringFactory)
        {
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
        }

        public Request GetRequest(long id)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var requestData = db.SingleFromProcedure("acvp.RequestGet", new
                {
                    RequestID = id
                });

                if (requestData == null)
                {
                    throw new Exception($"Unable to find request with id: {id}");
                }
                
                var result = new Request
                {
                    RequestID = id,
                    Status = requestData.Status,
                    ApprovedID = requestData.ApprovedID,
                    APIAction = requestData.APIActionID
                };

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting request");
                throw;
            }
        }
    }
}