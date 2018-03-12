using Newtonsoft.Json;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class OtherInput
    {
        [JsonProperty(PropertyName = "additionalInput")]
        public BitString AdditionalInput { get; set; }
        [JsonProperty(PropertyName = "entropyInput")]
        public BitString EntropyInput { get; set; }
    }
}
