using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public Capability[] Capabilities { get; set; }
    }

    public class Capability
    {
        public MathDomain IterationCount { get; set; }

        [JsonProperty(PropertyName = "passwordLen")]
        public MathDomain PasswordLength { get; set; }

        [JsonProperty(PropertyName = "saltLen")]
        public MathDomain SaltLength { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public MathDomain KeyLength { get; set; }

        [JsonProperty(PropertyName = "hmacAlg")]
        public string[] HashAlg { get; set; }
    }
}
