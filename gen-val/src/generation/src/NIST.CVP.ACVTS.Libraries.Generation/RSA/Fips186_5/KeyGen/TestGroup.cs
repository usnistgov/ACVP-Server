using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.KeyGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public bool InfoGeneratedByServer { get; set; }
        public int Modulo { get; set; }
        public string TestType { get; set; }

        public PrivateKeyModes KeyFormat { get; set; }

        [JsonProperty(PropertyName = "primeTest")]
        public PrimeTestModes PrimeTest { get; set; }

        [JsonProperty(PropertyName = "randPQ")]
        public PrimeGenModes PrimeGenMode { get; set; }

        public PublicExponentModes PubExp { get; set; }
        public BitString FixedPubExp { get; set; }

        [JsonIgnore]
        public HashFunction HashAlg { get; set; }

        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
