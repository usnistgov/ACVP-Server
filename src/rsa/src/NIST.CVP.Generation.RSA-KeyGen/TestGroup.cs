using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public bool InfoGeneratedByServer { get; set; }
        public int Modulo { get; set; }
        public BitString FixedPubExp { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        public string TestType { get; set; }

        public PrivateKeyModes KeyFormat { get; set; }
        public PrimeTestModes PrimeTest { get; set; }

        [JsonProperty(PropertyName = "randPQ")]
        public PrimeGenModes PrimeGenMode { get; set; }
        public PublicExponentModes PubExp { get; set; }

        [JsonIgnore]
        public HashFunction HashAlg { get; set; }

        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "primemethod":
                    PrimeGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(value);
                    return true;
                case "mod":
                    Modulo = int.Parse(value);
                    return true;
                case "hash":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;
                case "table for m-t test":
                    PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(value);
                    return true;
            }

            return false;
        }
    }
}
