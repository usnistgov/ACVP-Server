using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash
{
    public class AlgoArrayResponse
    {
        [JsonIgnore]
        public BitString Message { get; set; }
        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }
        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength => Digest.BitLength;

        [JsonIgnore] public bool ShouldPrintOutputLength { get; set; } = false;
    }
}
