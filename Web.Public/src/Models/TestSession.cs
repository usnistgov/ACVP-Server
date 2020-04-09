using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Web.Public.Models
{
    public class TestSession
    {
        [JsonIgnore]
        public long ID { get; set; }
        
        public string URL => $"/acvp/v1/testSessions/{ID}";
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        
        [JsonIgnore]
        public List<long> VectorSetIDs { get; set; }
        
        public List<string> VectorSetURLs => VectorSetIDs.Select(vsId => $"/acvp/v1/testSessions/{ID}/vectorSets/{vsId}").ToList();
        public bool Publishable { get; set; }
        public bool Passed { get; set; }
        public bool IsSample { get; set; }
        public string AccessToken { get; set; }
    }
}