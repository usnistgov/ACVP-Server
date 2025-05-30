using System;
using Newtonsoft.Json;

public class GenValResponseJob
{
        public Guid JobId { get; set; }           // Mirrors the request's JobId

        [JsonProperty("operation")]
        public string Operation { get; set; }

        // This is a serialized string (JSON array)
        [JsonProperty("result")]
         public string Result { get; set; }
         public bool Success { get; set; } = true;
         public string? Error { get; set; }
}