using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.GenVal
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public bool IsSample { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string[] Mode { get; set; }
        public int[] KeyLen { get; set; }
    }
}
