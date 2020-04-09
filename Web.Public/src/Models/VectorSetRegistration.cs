using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.Public.Models
{
    public class VectorSetRegistration
    {
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
        
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("revision")]
        public string Revision { get; set; }
        
        public bool IsSample { get; set; }
        
        public long VsID { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }
    }
}