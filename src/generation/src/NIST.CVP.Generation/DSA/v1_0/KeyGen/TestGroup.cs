using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.v1_0.KeyGen
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        public int L { get; set; }
        public int N { get; set; }
        
        /// <summary>
        /// Ignoring for (De)Serialization as PQG are flattened
        /// </summary>
        [JsonIgnore] public FfcDomainParameters DomainParams { get; set; } = new FfcDomainParameters();

        [JsonProperty(PropertyName = "p", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString P
        {
            get => DomainParams?.P != 0 ? new BitString(DomainParams.P, L) : null;
            set => DomainParams.P = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "q", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString Q
        {
            get => DomainParams?.Q != 0 ? new BitString(DomainParams.Q, N) : null;
            set => DomainParams.Q = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "g", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString G
        {
            get => DomainParams?.G != 0 ? new BitString(DomainParams.G, L) : null;
            set => DomainParams.G = value.ToPositiveBigInteger();
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "p":
                    P = new BitString(value).PadToModulusMsb(32);
                    return true;

                case "q":
                    Q = new BitString(value).PadToModulusMsb(32);
                    return true;

                case "g":
                    G = new BitString(value).PadToModulusMsb(32);
                    return true;

                case "l":
                    L = int.Parse(value);
                    return true;

                case "n":
                    N = int.Parse(value);
                    return true;
            }

            return false;
        }
    }
}
