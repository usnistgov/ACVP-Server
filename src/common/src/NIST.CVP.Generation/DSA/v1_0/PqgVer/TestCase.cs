using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.v1_0.PqgVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }
        public string Reason { get; set; }      // Needs to be a string because of PQFailureReasons type and GFailureReasons type

        public BitString P { get; set; }
        public BitString Q { get; set; }
        public BitString G { get; set; }
        public BitString H { get; set; }

        [JsonIgnore] public DomainSeed Seed { get; set; } = new DomainSeed();

        [JsonProperty(PropertyName = "domainSeed")]
        public BitString DomainSeed
        {
            get
            {
                // We assume the seed was generated using the probable pq approach, meaning Seed is a single N-len value (provable would mean the Seed is 3 N-len values)
                if (Seed.Seed == null)
                {
                    return null;
                }

                return new BitString(Seed.Seed.ToPositiveBigInteger(), ParentGroup.N, true);
            }
            set => Seed.Seed = value;
        }

        [JsonProperty(PropertyName = "pSeed")]
        public BitString PSeed
        {
            get
            {
                if (Seed.PSeed == null)
                {
                    return null;
                }

                return new BitString(Seed.PSeed.ToPositiveBigInteger(), ParentGroup.N, true);
            }
            set => Seed.PSeed = value;
        }

        [JsonProperty(PropertyName = "qSeed")]
        public BitString QSeed
        {
            get
            {
                if (Seed.QSeed == null)
                {
                    return null;
                }

                return new BitString(Seed.QSeed.ToPositiveBigInteger(), ParentGroup.N, true);
            }
            set => Seed.QSeed = value;
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
                    P = new BitString(value);
                    return true;

                case "q":
                    Q = new BitString(value);
                    return true;

                case "g":
                    G = new BitString(value);
                    return true;

                case "h":
                    H = new BitString(value);
                    return true;

                case "index":
                    Index = new BitString(value);
                    return true;

                case "seed":
                case "domain_parameter_seed":
                    Seed = new DomainSeed(new BitString(value));
                    return true;

                case "c":
                    Counter = new Counter(int.Parse(value));
                    return true;

                case "firstseed":
                    Seed.Seed = new BitString(value);
                    return true;

                case "pseed":
                    Seed.PSeed = new BitString(value);
                    return true;

                case "qseed":
                    Seed.QSeed = new BitString(value);
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
