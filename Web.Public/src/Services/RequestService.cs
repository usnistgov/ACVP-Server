using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Web.Public.Models;
using Web.Public.Providers;
using Request = Web.Public.Models.Request;

namespace Web.Public.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestProvider _requestProvider;

        public RequestService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public Request GetRequest(long id)
        {
            var request = _requestProvider.GetRequest(id);

            // If the request is null, check the "external" table as the request may not be replicated to retrieve the "initial" state.
            if (request == null)
            {
                var requestInitialized = _requestProvider.CheckRequestInitialized(id);
                if (requestInitialized)
                {
                    return new Request()
                    {
                        Status = RequestStatus.Initial,
                        RequestID = id
                    };
                }
                
                return null;
            }
            
            if (request.Status == RequestStatus.Approved)
            {
                request.ApprovedURL = BuildApprovedURL(request.ApprovedID, request.APIAction);
            }
            
            return request;
        }

        public List<Request> GetAllRequestsForUser(long userID)
        {
            var requests = _requestProvider.GetAllRequestsForUser(userID);

            foreach (var request in requests)
            {
                if (request.Status == RequestStatus.Approved)
                {
                    request.ApprovedURL = BuildApprovedURL(request.ApprovedID, request.APIAction);
                }
            }

            return requests;
        }

        private string BuildApprovedURL(long approvedID, APIAction apiAction)
        {
            // TODO make sure DELETE uses the same URL ? 
            var baseURL = "/acvp/v1";
            
            return apiAction switch
            {
                APIAction.CreateDependency => $"{baseURL}/dependencies/{approvedID}",
                APIAction.UpdateDependency => $"{baseURL}/dependencies/{approvedID}",
                APIAction.DeleteDependency => $"{baseURL}/dependencies/{approvedID}",
                
                APIAction.CreateImplementation => $"{baseURL}/implementations/{approvedID}",
                APIAction.UpdateImplementation => $"{baseURL}/implementations/{approvedID}",
                APIAction.DeleteImplementation => $"{baseURL}/implementations/{approvedID}",
                
                APIAction.CreateOE => $"{baseURL}/oes/{approvedID}",
                APIAction.UpdateOE => $"{baseURL}/oes/{approvedID}",
                APIAction.DeleteOE => $"{baseURL}/oes/{approvedID}",
                
                APIAction.CreatePerson => $"{baseURL}/persons/{approvedID}",
                APIAction.UpdatePerson => $"{baseURL}/persons/{approvedID}",
                APIAction.DeletePerson => $"{baseURL}/persons/{approvedID}",
                
                APIAction.CreateVendor => $"{baseURL}/organizations/{approvedID}",
                APIAction.UpdateVendor => $"{baseURL}/organizations/{approvedID}",
                APIAction.DeleteVendor => $"{baseURL}/organizations/{approvedID}",
                
                APIAction.RegisterTestSession => "",
                APIAction.CancelTestSession => "",
                APIAction.CertifyTestSession => "",
                APIAction.SubmitVectorSetResults => "",
                APIAction.CancelVectorSet => "",
                
                APIAction.Unknown => throw new Exception("Unable to find matching action"),
                _ => throw new Exception("Unable to find matching action")
            };
        }
    }
}