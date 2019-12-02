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

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }

        /// <summary>
        /// DomainSeed object represented as flat JSON structure
        /// </summary>
        [JsonIgnore] public DomainSeed Seed { get; set; } = new DomainSeed();
        [JsonProperty(PropertyName = "domainSeed")]
        public BitString DomainSeed
        {
            get => Seed.Seed != 0 ? new BitString(Seed.Seed).PadToModulusMsb(32) : null;
            set => Seed.Seed = value.ToPositiveBigInteger();
        }
        [JsonProperty(PropertyName = "pSeed")]
        public BitString PSeed
        {
            get => Seed.PSeed != 0 ? new BitString(Seed.PSeed).PadToModulusMsb(32) : null;
            set => Seed.PSeed = value.ToPositiveBigInteger();
        }
        [JsonProperty(PropertyName = "qSeed")]
        public BitString QSeed
        {
            get => Seed.QSeed != 0 ? new BitString(Seed.QSeed).PadToModulusMsb(32) : null;
            set => Seed.QSeed = value.ToPositiveBigInteger();
        }
        [JsonProperty(PropertyName = "fullSeed")]
        public BitString FullSeed
        {
            get => Seed.GetFullSeed() != 0 ? new BitString(Seed.GetFullSeed()).PadToModulusMsb(32) : null;
            set => Seed.Seed = value.ToPositiveBigInteger();
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

                case "index":
                    Index = new BitString(value);
                    return true;

                case "domain_parameter_seed":
                    Seed = new DomainSeed(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "counter":
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
            }

            return false;
        }
    }
}
