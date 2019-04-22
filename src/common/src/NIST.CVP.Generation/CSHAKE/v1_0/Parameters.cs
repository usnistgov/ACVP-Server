using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CSHAKE.v1_0
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        
        // Was "digestSizes" but client only will send one at a time
        [JsonProperty(PropertyName = "digestSize")]
        public int[] DigestSizes { get; set; }

        [JsonProperty(PropertyName = "hexCustomization")]
        public bool HexCustomization { get; set; } = false;

        // Hard assumption that this is just a single RangeSegment inside of a Domain
        [JsonProperty(PropertyName = "outputLength")]
        public MathDomain OutputLength { get; set; }

        // Hard assumption that this is just a single RangeSegment inside of a Domain
        [JsonProperty(PropertyName = "msgLength")]
        public MathDomain MessageLength { get; set; }
    }
}
