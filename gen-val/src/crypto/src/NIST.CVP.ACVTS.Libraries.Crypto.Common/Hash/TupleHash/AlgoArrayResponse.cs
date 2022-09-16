using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash
{
    public class AlgoArrayResponse
    {
        [JsonIgnore]
        public List<BitString> Tuple { get; set; }
        [JsonIgnore]
        public string Customization { get; set; }

        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }

        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength => Digest.BitLength;
    }
}
