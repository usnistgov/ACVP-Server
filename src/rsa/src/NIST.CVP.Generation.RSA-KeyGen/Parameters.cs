using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public bool InfoGeneratedByServer { get; set; } = true;
        public string PubExpMode { get; set; }
        public string FixedPubExp { get; set; } = "";
        public string KeyFormat { get; set; }

        [JsonProperty(PropertyName = "algSpecs")]
        public AlgSpec[] AlgSpecs;
    }

    public class AlgSpec
    {
        [JsonProperty(PropertyName = "randPQ")]
        public string RandPQ;

        [JsonProperty(PropertyName = "capabilities")]
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
