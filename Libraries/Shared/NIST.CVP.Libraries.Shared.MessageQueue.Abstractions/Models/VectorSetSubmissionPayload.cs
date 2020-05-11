using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
    public class VectorSetSubmissionPayload : IMessagePayload
    {
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
        
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("revision")]
        public string Revision { get; set; }
        
        [JsonPropertyName("vsId")]
        public long VectorSetID { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }
}