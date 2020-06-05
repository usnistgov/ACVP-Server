using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace Web.Public.Models
{
    public class TestSession
    {
        [JsonIgnore]
        public long ID { get; set; }
        
        [JsonPropertyName("url")]
        public string URL => $"/acvp/v1/testSessions/{ID}";
        
        [JsonPropertyName("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("expiresOn")]
        public DateTime ExpiresOn { get; set; }
        
        [JsonIgnore]
        public List<long> VectorSetIDs { get; set; }
        
        [JsonPropertyName("vectorSetUrls")]
        public List<string> VectorSetURLs => VectorSetIDs.Select(vsId => $"/acvp/v1/testSessions/{ID}/vectorSets/{vsId}").ToList();
        
        [JsonPropertyName("publishable")]
        public bool Publishable => !IsSample;

        [JsonPropertyName("passed")]
        public bool Passed => Status == TestSessionStatus.Passed;

        [JsonPropertyName("isSample")]
        public bool IsSample { get; set; }

        public string AccessToken { get; set; }

        [JsonIgnore]
        public TestSessionStatus Status { get; set; }

        [JsonIgnore]
        public DateTime LastTouched { get; set; }
    }
}