using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.v1_0.PqgGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        [JsonIgnore]
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }

        public BitString P { get; set; }
        public BitString Q { get; set; }
        public BitString G { get; set; }

        /// <summary>
        /// DomainSeed object represented as flat JSON structure
        /// </summary>
        [JsonIgnore] public DomainSeed Seed { get; set; } = new DomainSeed();
        [JsonProperty(PropertyName = "domainSeed")]
        public BitString DomainSeed
        {
            get => Seed.Seed;
            set => Seed.Seed = value;
        }
        [JsonProperty(PropertyName = "pSeed")]
        public BitString PSeed
        {
            get => Seed.PSeed;
            set => Seed.PSeed = value;
        }
        [JsonProperty(PropertyName = "qSeed")]
        public BitString QSeed
        {
            get => Seed.QSeed;
            set => Seed.QSeed = value;
        }
        [JsonProperty(PropertyName = "fullSeed")]
        public BitString FullSeed
        {
            get => Seed.GetFullSeed();
            set => Seed.Seed = value;
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

                case "index":
                    Index = new BitString(value);
                    return true;

                case "domain_parameter_seed":
                    Seed = new DomainSeed(new BitString(value));
                    return true;

                case "counter":
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
            }

            return false;
        }
    }
}
