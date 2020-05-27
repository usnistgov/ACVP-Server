using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.External;

namespace Web.Public.JsonObjects
{
    public class AlgorithmListObject
    {
        [JsonPropertyName("algorithms")]
        public IEnumerable<AlgorithmBase> AlgorithmList { get; set; }
    }
}