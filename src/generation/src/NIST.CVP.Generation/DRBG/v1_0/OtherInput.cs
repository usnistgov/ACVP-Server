using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG.v1_0
{
    public class OtherInput
    {
        [JsonProperty(PropertyName = "intendedUse")]
        public string IntendedUse { get; set; }
		[JsonProperty(PropertyName = "additionalInput")]
        public BitString AdditionalInput { get; set; }
        [JsonProperty(PropertyName = "entropyInput")]
        public BitString EntropyInput { get; set; }
    }
}
