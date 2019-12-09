using NIST.CVP.Math;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public class AlgoArrayResponse
    {
        public List<BitString> Tuple { get; set; }
        public string Customization { get; set; }
        
        [JsonProperty(PropertyName = "md")]
        public BitString Digest { get; set; }
        
        [JsonProperty(PropertyName = "outLen")]
        public int DigestLength => Digest.BitLength;
    }
}
