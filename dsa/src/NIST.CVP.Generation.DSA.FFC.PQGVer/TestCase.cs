using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }
        public string Reason { get; set; }
        public bool Result { get; set; }

        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger G { get; set; }
        public BigInteger H { get; set; }
        public DomainSeed Seed { get; set; }
        public Counter Counter { get; set; }
        public BitString Index { get; set; }

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

            if (((ExpandoObject)source).ContainsProperty("result"))
            {
                Result = (bool)source.result;
            }

            if (((ExpandoObject)source).ContainsProperty("failureTest"))
            {
                FailureTest = (bool)source.failureTest;
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
                Reason = otherTypedTest.Reason;
                FailureTest = otherTypedTest.FailureTest;

                return true;
            }

            if (G == null && otherTypedTest.G != null)
            {
                Reason = otherTypedTest.Reason;
                FailureTest = otherTypedTest.FailureTest;

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
