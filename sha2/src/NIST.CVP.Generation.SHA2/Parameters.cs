using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "function")]
        public string[] Mode { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public string[] DigestSize { get; set; }

        [JsonProperty(PropertyName = "bitOriented")]
        public bool BitOriented { get; set; } = false;

        [JsonProperty(PropertyName = "includeNull")]
        public bool IncludeNull { get; set; } = false;
    }
}
