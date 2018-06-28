using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int[] DigestSizes { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "outBit")]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; } = false;

        [JsonProperty(PropertyName = "xof")]
        public bool XOF { get; set; }

        public MathDomain KeyLen { get; set; }
        public MathDomain MacLen { get; set; }
    }
}
