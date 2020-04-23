using System.Collections.Generic;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;

namespace Web.Public.Models
{
    public class TestSessionRegistration : IJsonObject
    {
        [JsonPropertyName("tsID")]
        public long ID { get; set; }
        
        [JsonPropertyName("isSample")]
        public bool IsSample { get; set; }
        
        [JsonPropertyName("algorithms")]
        public List<VectorSetRegistration> Algorithms { get; set; }
        
        public List<string> ValidateObject()
        {
            var errors = new List<string>();

            if (Algorithms == null || Algorithms.Count == 0)
            {
                errors.Add("No algorithms provided.");
            }

            return errors;
        }
    }
}