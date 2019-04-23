using System;
using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core
{
    public class ParameterCheckResponse
    {
        public StatusCode StatusCode { get; }
        public string ErrorMessage { get; }

        public ParameterCheckResponse()
        {
            StatusCode = StatusCode.Success;
        }

        public ParameterCheckResponse(string errorMessage, StatusCode statusCode = StatusCode.ParameterValidationError)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
