using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models
{
    public class VectorSetSubmissionPayload : IWorkflowItemPayload
    {
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
        
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("revision")]
        public string Revision { get; set; }
        
        [JsonPropertyName("vsId")]
        public long VsID { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }
}