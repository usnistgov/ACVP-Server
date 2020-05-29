using System.Collections.Generic;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;

namespace Web.Public.Models
{
    public class VectorSetResult : IJsonObject
    {
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
        
        [JsonPropertyName("mode")]
        public string Mode { get; set; }
        
        [JsonPropertyName("revision")]
        public string Revision { get; set; }
        
        public long VsID { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> Properties { get; set; }

        public List<string> ValidateObject()
        {
            return new List<string>();
        }
    }
}