using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class AlgoArrayResponse
    {
        [JsonIgnore]
        public BitString Message { get; set; }
        [JsonProperty(PropertyName = "sig")]
        public BitString Signature { get; set; }
    }
}
