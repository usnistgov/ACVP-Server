using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigVer
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        // DEPRECATED gl279, needs removal eventually
        [JsonProperty(PropertyName = "componentTest")]
        public bool Component { get; set; }

        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        public string[] Curve { get; set; }
        public string[] HashAlg { get; set; }
    }
}
