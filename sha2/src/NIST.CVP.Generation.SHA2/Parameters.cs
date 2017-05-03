using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public string[] DigestSizes { get; set; }
        
        [JsonProperty(PropertyName = "inBit")]
        public string BitOriented { get; set; } = "no";

        [JsonProperty(PropertyName = "inEmpty")]
        public string IncludeNull { get; set; } = "no";
    }
}
