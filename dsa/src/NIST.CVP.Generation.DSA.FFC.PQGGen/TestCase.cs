using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
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

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            if (((ExpandoObject)source).ContainsProperty("p"))
            {
                P = BigIntegerFromObject("p", (ExpandoObject)source);
            }

            if (((ExpandoObject)source).ContainsProperty("q"))
            {
                Q = BigIntegerFromObject("q", (ExpandoObject)source);
            }

            if (((ExpandoObject)source).ContainsProperty("g"))
            {
                G = BigIntegerFromObject("g", (ExpandoObject)source);
            }

            if (((ExpandoObject)source).ContainsProperty("domainSeed"))
            {
                if (((ExpandoObject)source).ContainsProperty("pSeed") && ((ExpandoObject)source).ContainsProperty("qSeed"))
                {
                    var firstSeed = BigIntegerFromObject("domainSeed", (ExpandoObject)source);
                    var pSeed = BigIntegerFromObject("pSeed", (ExpandoObject)source);
                    var qSeed = BigIntegerFromObject("qSeed", (ExpandoObject)source);

                    Seed = new DomainSeed(firstSeed, pSeed, qSeed);
                }
                else
                {
                    Seed = new DomainSeed(BigIntegerFromObject("domainSeed", (ExpandoObject)source));
                }
            }

            if (((ExpandoObject)source).ContainsProperty("counter"))
            {
                Counter = new Counter((int)source.counter);
            }

            if (((ExpandoObject)source).ContainsProperty("pcounter") && ((ExpandoObject)source).ContainsProperty("qcounter"))
            {
                Counter = new Counter((int)source.pcounter, (int)source.qcounter);
            }

            if (((ExpandoObject)source).ContainsProperty("index"))
            {
                Index = BitStringFromObject("index", (ExpandoObject)source);
            }
        }

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)otherTest;

            if (P == null && otherTypedTest.P != null)
            {
                P = otherTypedTest.P;
                Q = otherTypedTest.Q;
                Seed = otherTypedTest.Seed;
                Counter = otherTypedTest.Counter;

                return true;
            }

            if (G == null && otherTypedTest.G != null)
            {
                G = otherTypedTest.G;
                Index = otherTypedTest.Index.GetDeepCopy();

                return true;
            }

            return false;
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

        private BitString BitStringFromObject(string propName, ExpandoObject source)
        {
            if (!source.ContainsProperty(propName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[propName];
            if (sourcePropertyValue == null)
            {
                return null;
            }

            if (sourcePropertyValue is BitString valueAsBitString)
            {
                return valueAsBitString;
            }

            return new BitString(sourcePropertyValue.ToString());
        }

        private BigInteger BigIntegerFromObject(string sourcePropertyName, ExpandoObject source)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return 0;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return 0;
            }

            if (sourcePropertyValue.GetType() == typeof(string))
            {
                return new BitString(sourcePropertyValue.ToString()).ToPositiveBigInteger();
            }

            var valueAsBigInteger = (BigInteger)sourcePropertyValue;
            if (valueAsBigInteger != 0)
            {
                return valueAsBigInteger;
            }

            return 0;
        }
    }
}
