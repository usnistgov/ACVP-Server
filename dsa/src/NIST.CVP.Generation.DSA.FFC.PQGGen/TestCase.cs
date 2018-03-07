using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }
        public DomainSeed Seed { get; set; }
        public Counter Counter { get; set; }
        public BitString Index { get; set; }

        // Used for SetString only
        private BigInteger _firstSeed;
        private BigInteger _pSeed;
        private BigInteger _qSeed;
        private int _pCounter;
        private int _qCounter;

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            
            var expandoSource = (ExpandoObject) source;
            P = expandoSource.GetBigIntegerFromProperty("p");
            Q = expandoSource.GetBigIntegerFromProperty("q");
            G = expandoSource.GetBigIntegerFromProperty("g");

            var firstSeed = expandoSource.GetBigIntegerFromProperty("domainSeed");
            var pSeed = expandoSource.GetBigIntegerFromProperty("pSeed");
            var qSeed = expandoSource.GetBigIntegerFromProperty("qSeed");

            if (pSeed == default(BigInteger) && qSeed == default(BigInteger))
            {
                Seed = new DomainSeed(firstSeed);
            }
            else
            {
                Seed = new DomainSeed(firstSeed, pSeed, qSeed);
            }

            var counter = expandoSource.GetTypeFromProperty<int>("counter");
            var pCounter = expandoSource.GetTypeFromProperty<int>("pCounter");
            var qCounter = expandoSource.GetTypeFromProperty<int>("qCounter");
            if (counter != default(int))
            {
                Counter = new Counter(counter);
            }
            else
            {
                Counter = new Counter(pCounter, qCounter);
            }

            Index = expandoSource.GetBitStringFromProperty("index");
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
                    _firstSeed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "pseed":
                    _pSeed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qseed":
                    _qSeed = new BitString(value).ToPositiveBigInteger();
                    Seed = new DomainSeed(_firstSeed, _pSeed, _qSeed);
                    return true;

                case "pgen_counter":
                    _pCounter = int.Parse(value);
                    return true;

                case "qgen_counter":
                    _qCounter = int.Parse(value);
                    Counter = new Counter(_pCounter, _qCounter);
                    return true;
            }

            return false;
        }
    }
}
