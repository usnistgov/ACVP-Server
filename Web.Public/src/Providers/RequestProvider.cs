using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Request> GetAllRequestsForUser(long userID)
        {
            var db = new MightyOrm(_connectionString);

            try
            {
                var requestData = db.QueryFromProcedure("acvp.RequestGetFromUser", new
                {
                    UserID = userID
                });

                if (requestData == null)
                {
                    throw new Exception($"Unable to find requests for user id: {userID}");
                }

                return requestData.Select(request => new Request {RequestID = request.ID, Status = request.Status, ApprovedID = request.ApprovedID, APIAction = request.APIActionID}).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting requests");
                throw;
            }
        }
    }
}