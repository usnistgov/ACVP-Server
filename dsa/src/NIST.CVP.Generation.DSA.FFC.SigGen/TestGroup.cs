using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        [JsonIgnore] public FfcDomainParameters DomainParams { get; set; } = new FfcDomainParameters();

        [JsonProperty(PropertyName = "p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger P
        {
            get => DomainParams.P;
            set => DomainParams.P = value;
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger Q
        {
            get => DomainParams.Q;
            set => DomainParams.Q = value;
        }

        [JsonProperty(PropertyName = "g", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger G
        {
            get => DomainParams.G;
            set => DomainParams.G = value;
        }

        [JsonIgnore] public HashFunction HashAlg { get; set; }
        [JsonProperty(PropertyName = "hashAlg")]
        public string HashAlgName
        {
            get => HashAlg?.Name;
            set => HashAlg = ShaAttributes.GetHashFunctionFromName(value);
        }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "p":
                    DomainParams.P = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    DomainParams.Q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "g":
                    DomainParams.G = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;

                case "hashalg":
                    HashAlgName = value;
                    return true;
            }

            return false;
        }
    }
}
