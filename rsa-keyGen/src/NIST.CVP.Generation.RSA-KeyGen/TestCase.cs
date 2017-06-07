using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }   // Not used
        public bool Deferred { get; set; }

        public KeyPair Key { get; set; }
        public BitString Seed { get; set; }

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

        public bool Merge(ITestCase otherTest)
        {
            if (TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase) otherTest;
            var retVal = false;

            if (Key.PubKey.E == 0 && otherTypedTest.Key.PubKey.E != 0)
            {
                Key.PubKey.E = otherTypedTest.Key.PubKey.E;
                retVal = true;
            }

            if (Key.PubKey.N == 0 && otherTypedTest.Key.PubKey.N != 0)
            {
                Key.PubKey.N = otherTypedTest.Key.PubKey.N;
                retVal = true;
            }

            if (Key.PrivKey.P == 0 && otherTypedTest.Key.PrivKey.P != 0)
            {
                Key.PrivKey.P = otherTypedTest.Key.PrivKey.P;
                retVal = true;
            }

            if (Key.PrivKey.Q == 0 && otherTypedTest.Key.PrivKey.Q != 0)
            {
                Key.PrivKey.Q = otherTypedTest.Key.PrivKey.Q;
                retVal = true;
            }

            if (Key.PrivKey.D == 0 && otherTypedTest.Key.PrivKey.D != 0)
            {
                Key.PrivKey.D = otherTypedTest.Key.PrivKey.D;
                retVal = true;
            }

            return retVal;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int) source.tcId;
            if (((ExpandoObject) source).ContainsProperty("deferred"))
            {
                Deferred = source.deferred;
            }

            Key = KeyPairFromObject((ExpandoObject) source);
            Seed = BitStringFromObject("seed", (ExpandoObject) source);
        }

        private KeyPair KeyPairFromObject(ExpandoObject source)
        {
            var e = BitStringFromObject("e", source);
            var p = BitStringFromObject("p", source);
            var q = BitStringFromObject("q", source);
            var n = BitStringFromObject("n", source);
            var d = BitStringFromObject("d", source);

            return new KeyPair(p, q, n, e, d);
        }

        private BitString BitStringFromObject(string sourcePropertyName, ExpandoObject source)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];
            if (sourcePropertyValue == null)
            {
                return null;
            }

            var valueAsBitString = sourcePropertyValue as BitString;
            if (valueAsBitString != null)
            {
                return valueAsBitString;
            }

            return new BitString(sourcePropertyValue.ToString());
        }
    }
}
