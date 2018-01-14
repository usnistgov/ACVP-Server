using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public string Reason { get; set; }      // Needs to be a string because of PQFailureReasons type and GFailureReasons type
        public bool Result { get; set; }

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }
        public BigInteger H { get; set; }
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

            ExpandoObject expandoSource = (ExpandoObject)source;
            var resultValue = expandoSource.GetTypeFromProperty<string>("result");
            if (resultValue != null)
            {
                Result = resultValue.ToLower() == "passed";
            }
            Reason = expandoSource.GetTypeFromProperty<string>("reason");
        }

        public bool Merge(ITestCase otherTest)
        {
            // We don't need any properties from the prompt...
            return (TestCaseId == otherTest.TestCaseId);
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

                case "result":
                    Result = (value.StartsWith("p"));
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
