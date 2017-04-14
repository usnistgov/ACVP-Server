using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "functions")]
        public Function[] Functions { get; set; }
        
        [JsonProperty(PropertyName = "bitOriented")]
        public bool BitOriented { get; set; } = false;

        [JsonProperty(PropertyName = "includeNull")]
        public bool IncludeNull { get; set; } = false;
    }
}
