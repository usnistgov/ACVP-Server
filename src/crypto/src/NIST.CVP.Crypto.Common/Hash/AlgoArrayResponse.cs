using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash
{
    public class AlgoArrayResponse
    {
        [JsonProperty(PropertyName = "msg")]
        public BitString Message { get; set; }
        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }
        public int DigestLength => Digest.BitLength;
    }
}
