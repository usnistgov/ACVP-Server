using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public bool InfoGeneratedByServer { get; set; } = true;
        public string PubExpMode { get; set; }
        public string FixedPubExp { get; set; } = "";
        public string KeyFormat { get; set; }

        [JsonProperty(PropertyName = "capabilities")]
        public AlgSpec[] AlgSpecs;
    }

    public class AlgSpec
    {
        [JsonProperty(PropertyName = "randPQ")]
        public string RandPQ;

        [JsonProperty(PropertyName = "properties")]
        public Capability[] Capabilities;
    }

    public class Capability
    {
        [JsonProperty(PropertyName = "modulo")]
        public int Modulo;

        [JsonProperty(PropertyName = "hashAlg")]
        public string[] HashAlgs;

        [JsonProperty(PropertyName = "primeTest")]
        public string[] PrimeTests;
    }
}
