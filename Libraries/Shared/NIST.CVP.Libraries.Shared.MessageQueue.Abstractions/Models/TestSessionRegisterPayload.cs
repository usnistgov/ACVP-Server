using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
    public class TestSessionRegisterPayload : IMessagePayload
    {
        [JsonPropertyName("tsId")]
        public long ID { get; set; }

        [JsonPropertyName("acvVersion")] 
        public string AcvVersion { get; set; } = "1.0";
        
        [JsonPropertyName("isSample")]
        public bool IsSample { get; set; }
        
        [JsonPropertyName("algorithms")]
        public List<VectorSetRegisterPayload> Algorithms { get; set; }
    }
    
    public class VectorSetRegisterPayload
    {
        [JsonPropertyName(("vsId"))]
        public long VsID { get; set; }
        
        [JsonPropertyName("isSample")]
        public bool IsSample { get; set; }
        
        [JsonPropertyName("algorithmId")]
        public long AlgorithmId { get; set; }
        
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
        
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("revision")]
        public string Revision { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }
}