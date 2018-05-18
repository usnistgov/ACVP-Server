using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Rest.TransientFaultHandling;
using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core
{
    public class ValidateResponse
    {
        public StatusCode StatusCode { get; }
        public string ErrorMessage { get; }

        public ValidateResponse()
        {
            StatusCode = StatusCode.Success;
        }

        public ValidateResponse(string errorMessage, StatusCode statusCode = StatusCode.ValidatorError)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
