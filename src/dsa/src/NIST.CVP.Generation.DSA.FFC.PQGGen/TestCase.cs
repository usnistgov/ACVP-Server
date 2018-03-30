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
        // Used for SetString only
        private BigInteger firstSeed;
        private BigInteger pSeed;
        private BigInteger qSeed;
        private int pCounter;
        private int qCounter;

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

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }
        public DomainSeed Seed { get; set; }
        public Counter Counter { get; set; }
        public BitString Index { get; set; }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;
            var expandoSource = (ExpandoObject) source;

            if (expandoSource.ContainsProperty("p"))
            {
                P = expandoSource.GetBigIntegerFromProperty("p");
            }

            if (expandoSource.ContainsProperty("q"))
            {
                Q = expandoSource.GetBigIntegerFromProperty("q");
            }

            if (expandoSource.ContainsProperty("g"))
            {
                G = expandoSource.GetBigIntegerFromProperty("g");
            }

            if (expandoSource.ContainsProperty("domainSeed"))
            {
                if (expandoSource.ContainsProperty("pSeed") && expandoSource.ContainsProperty("qSeed"))
                {
                    var firstSeed = expandoSource.GetBigIntegerFromProperty("domainSeed");
                    var pSeed = expandoSource.GetBigIntegerFromProperty("pSeed");
                    var qSeed = expandoSource.GetBigIntegerFromProperty("qSeed");

                    Seed = new DomainSeed(firstSeed, pSeed, qSeed);
                }
                else
                {
                    Seed = new DomainSeed(expandoSource.GetBigIntegerFromProperty("domainSeed"));
                }
            }

            if (expandoSource.ContainsProperty("counter"))
            {
                Counter = new Counter((int)source.counter);
            }

            if (expandoSource.ContainsProperty("pCounter") && expandoSource.ContainsProperty("qCounter"))
            {
                Counter = new Counter((int)source.pCounter, (int)source.qCounter);
            }

            if (((ExpandoObject)source).ContainsProperty("index"))
            {
                Index = expandoSource.GetBitStringFromProperty("index");
            }
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
                    firstSeed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "pseed":
                    pSeed = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "qseed":
                    qSeed = new BitString(value).ToPositiveBigInteger();
                    Seed = new DomainSeed(firstSeed, pSeed, qSeed);
                    return true;

                case "pgen_counter":
                    pCounter = int.Parse(value);
                    return true;

                case "qgen_counter":
                    qCounter = int.Parse(value);
                    Counter = new Counter(pCounter, qCounter);
                    return true;
            }

            return false;
        }
    }
}
