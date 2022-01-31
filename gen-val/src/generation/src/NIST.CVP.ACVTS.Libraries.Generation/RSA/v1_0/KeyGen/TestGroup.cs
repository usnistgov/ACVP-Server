using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public bool InfoGeneratedByServer { get; set; }
        public int Modulo { get; set; }
        public BitString FixedPubExp { get; set; }
        public string TestType { get; set; }

        public PrivateKeyModes KeyFormat { get; set; }

        [JsonProperty(PropertyName = "primeTest")]
        public PrimeTestFips186_4Modes PrimeTest { get; set; }

        [JsonProperty(PropertyName = "randPQ")]
        public PrimeGenFips186_4Modes PrimeGenMode { get; set; }

        public PublicExponentModes PubExp { get; set; }

        [JsonIgnore]
        public HashFunction HashAlg { get; set; }

        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "primemethod":
                    PrimeGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenFips186_4Modes>(value);
                    return true;
                case "mod":
                    Modulo = int.Parse(value);
                    return true;
                case "hash":
                    HashAlg = ShaAttributes.GetHashFunctionFromName(value);
                    return true;
                case "table for m-t test":
                case "table for m-r test":
                    if (value.ToLower() == "c.2")
                    {
                        PrimeTest = PrimeTestFips186_4Modes.TblC2;
                    }
                    else if (value.ToLower() == "c.3")
                    {
                        PrimeTest = PrimeTestFips186_4Modes.TblC3;
                    }
                    return true;
            }

            return false;
        }
    }
}
