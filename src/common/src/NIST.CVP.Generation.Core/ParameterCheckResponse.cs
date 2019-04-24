using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core
{
    public class ParameterCheckResponse
    {
        [JsonProperty(PropertyName = "status")]
        public StatusCode StatusCode { get; }

        [JsonProperty(PropertyName = "errors")]
        public List<string> ErrorMessage { get; } = new List<string>();

        public ParameterCheckResponse()
        {
            StatusCode = StatusCode.Success;
        }

        public ParameterCheckResponse(string errorMessage, StatusCode statusCode = StatusCode.ParameterValidationError)
        {
            ErrorMessage.Add(errorMessage);
            StatusCode = statusCode;
        }

        [JsonIgnore]
        public bool Success => !ErrorMessage.Any();
    }
}
