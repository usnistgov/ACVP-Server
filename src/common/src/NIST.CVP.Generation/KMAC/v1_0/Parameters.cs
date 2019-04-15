using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        [JsonProperty(PropertyName = "digestSize")]
        public int[] DigestSizes { get; set; }

        [JsonProperty(PropertyName = "nonxof")]
        public bool NonXOF { get; set; } = true;

        [JsonProperty(PropertyName = "xof")]
        public bool XOF { get; set; }
        
        [JsonProperty(PropertyName = "hexCustomization")]
        public bool HexCustomization { get; set; } = false;

        public MathDomain MsgLen { get; set; }
        public MathDomain KeyLen { get; set; }
        public MathDomain MacLen { get; set; }
    }
}
