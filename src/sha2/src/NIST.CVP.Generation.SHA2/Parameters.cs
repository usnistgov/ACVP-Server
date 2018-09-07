using System;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        [JsonProperty(PropertyName = "digestSize")]
        public string[] DigestSizes { get; set; }
        
        [JsonProperty(PropertyName = "inBit")]
        public bool BitOriented { get; set; } = false;

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; } = false;
    }
}
