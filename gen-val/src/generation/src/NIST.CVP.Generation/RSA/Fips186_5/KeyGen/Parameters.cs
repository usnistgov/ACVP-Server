using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen
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
        public PublicExponentModes PubExpMode { get; set; }
        public BitString FixedPubExp { get; set; }
        public PrivateKeyModes KeyFormat { get; set; }

        [JsonProperty(PropertyName = "capabilities")]
        public AlgSpec[] AlgSpecs;
    }

    public class AlgSpec
    {
        [JsonProperty(PropertyName = "randPQ")]
        public PrimeGenModes RandPQ;

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
        public PrimeTestModes[] PrimeTests;

        [JsonProperty(PropertyName = "pMod8")]
        public int PMod8;

        [JsonProperty(PropertyName = "qMod8")] 
        public int QMod8;
    }
}