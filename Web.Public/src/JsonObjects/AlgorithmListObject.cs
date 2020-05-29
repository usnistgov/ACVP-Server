using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class AlgorithmListObject
    {
        [JsonPropertyName("algorithms")]
        public IEnumerable<AlgorithmObject> AlgorithmList { get; set; }
    }
}