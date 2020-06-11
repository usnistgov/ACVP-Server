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
                
                if (request.APIAction == APIAction.CertifyTestSession)
                {
                    request.ValidationId = request.ApprovedID;
                }
            }
            
            return request;
        }

        public (long TotalCount, List<Request> Requests) GetPagedRequestsForUser(long userID, PagingOptions pagingOptions)
        {
            var result = _requestProvider.GetPagedRequestsForUser(userID, pagingOptions.Offset, pagingOptions.Limit);

            foreach (var request in result.Requests)
            {
                if (request.Status == RequestStatus.Approved)
                {
                    request.ApprovedURL = BuildApprovedURL(request.ApprovedID, request.APIAction);
                    
                    if (request.APIAction == APIAction.CertifyTestSession)
                    {
                        request.ValidationId = request.ApprovedID;
                    }
                }
            }

            return result;
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
                
                APIAction.CreateImplementation => $"{baseURL}/modules/{approvedID}",
                APIAction.UpdateImplementation => $"{baseURL}/modules/{approvedID}",
                APIAction.DeleteImplementation => $"{baseURL}/modules/{approvedID}",
                
                APIAction.CreateOE => $"{baseURL}/oes/{approvedID}",
                APIAction.UpdateOE => $"{baseURL}/oes/{approvedID}",
                APIAction.DeleteOE => $"{baseURL}/oes/{approvedID}",
                
                APIAction.CreatePerson => $"{baseURL}/persons/{approvedID}",
                APIAction.UpdatePerson => $"{baseURL}/persons/{approvedID}",
                APIAction.DeletePerson => $"{baseURL}/persons/{approvedID}",
                
                APIAction.CreateVendor => $"{baseURL}/vendors/{approvedID}",
                APIAction.UpdateVendor => $"{baseURL}/vendors/{approvedID}",
                APIAction.DeleteVendor => $"{baseURL}/vendors/{approvedID}",
                
                APIAction.RegisterTestSession => null,
                APIAction.CancelTestSession => null,
                APIAction.CertifyTestSession => null,
                APIAction.SubmitVectorSetResults => null,
                APIAction.CancelVectorSet => null,
                
                APIAction.Unknown => null,
                _ => null
            };
        }
    }
}