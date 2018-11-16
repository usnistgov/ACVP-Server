using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }
        public string Reason { get; set; }      // Needs to be a string because of PQFailureReasons type and GFailureReasons type

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }
        public BigInteger H { get; set; }

        [JsonIgnore] public DomainSeed Seed { get; set; } = new DomainSeed();

        [JsonProperty(PropertyName = "domainSeed")]
        public BitString DomainSeed
        {
            get
            {
                if (ParentGroup?.GGenMode == GeneratorGenMode.Unverifiable || ParentGroup?.GGenMode == GeneratorGenMode.Canonical)
                {
                    return new BitString(Seed.GetFullSeed(), ParentGroup.N * 3, false);
                }

                if (Seed.Seed == 0)
                {
                    return null;
                }

                return new BitString(Seed.Seed, ParentGroup.N, false);
            }
            set => Seed.Seed = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "pSeed")]
        public BitString PSeed
        {
            get
            {
                if (Seed.PSeed == 0)
                {
                    return null;
                }

                return new BitString(Seed.PSeed, ParentGroup.N, false);
            }
            set => Seed.PSeed = value.ToPositiveBigInteger();
        }

        [JsonProperty(PropertyName = "qSeed")]
        public BitString QSeed
        {
            get
            {
                if (Seed.QSeed == 0)
                {
                    return null;
                }

                return new BitString(Seed.QSeed, ParentGroup.N, false);
            }
            set => Seed.QSeed = value.ToPositiveBigInteger();
        }

        [JsonIgnore] public Counter Counter { get; set; } = new Counter();
        [JsonProperty(PropertyName = "counter")]
        public int Count
        {
            get => Counter.Count;
            set => Counter.Count = value;
        }
        [JsonProperty(PropertyName = "pCounter")]
        public int PCount
        {
            get => Counter.PCount;
            set => Counter.PCount = value;
        }
        [JsonProperty(PropertyName = "QCounter")]
        public int QCount
        {
            get => Counter.QCount;
            set => Counter.QCount = value;
        }

        [JsonProperty(PropertyName = "index")]
        public BitString Index { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "p":
                    P = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    Q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "g":
                    G = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "h":
                    H = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "index":
                    Index = new BitString(value);
                    return true;

                case "seed":
                case "domain_parameter_seed":
                    Seed = new DomainSeed(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "c":
                    Counter = new Counter(int.Parse(value));
                    return true;

                case "firstseed":
                    Seed.Seed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "pseed":
                    Seed.PSeed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qseed":
                    Seed.QSeed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "pgen_counter":
                    Counter.PCount = int.Parse(value);
                    return true;

                case "qgen_counter":
                    Counter.QCount = int.Parse(value);
                    return true;

                case "result":
                    TestPassed = value.StartsWith("p");
                    return true;
            }

            return false;
        }
    }
}
