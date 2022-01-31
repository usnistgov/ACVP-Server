using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.Hash_DF
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public Capabilities[] Capabilities { get; set; }
    }

    public class Capabilities
    {
        [JsonProperty(PropertyName = "payloadLen")]
        public MathDomain PayloadLen { get; set; }
        public string[] HashAlg { get; set; }
    }
}
