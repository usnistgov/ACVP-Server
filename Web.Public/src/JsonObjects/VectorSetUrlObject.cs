using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class VectorSetUrlObject
    {
        [JsonPropertyName("vectorSetUrls")]
        public List<string> VectorSetURLs { get; set; }
    }
}