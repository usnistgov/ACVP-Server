using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
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
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public KeyPair Key { get; set; }
        public BitString Seed { get; set; }
        public int[] Bitlens { get; set; }

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

            if (Key.PrivKey.DMP1 == 0 && otherTypedTest.Key.PrivKey.DMP1 != 0)
            {
                Key.PrivKey.DMP1 = otherTypedTest.Key.PrivKey.DMP1;
                retVal = true;
            }

            if (Key.PrivKey.DMQ1 == 0 && otherTypedTest.Key.PrivKey.DMQ1 != 0)
            {
                Key.PrivKey.DMQ1 = otherTypedTest.Key.PrivKey.DMQ1;
                retVal = true;
            }

            if (Key.PrivKey.IQMP == 0 && otherTypedTest.Key.PrivKey.IQMP != 0)
            {
                Key.PrivKey.IQMP = otherTypedTest.Key.PrivKey.IQMP;
                retVal = true;
            }

            return retVal;
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "e":
                    Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "n":
                    Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "p":
                    Key.PrivKey.P = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "q":
                    Key.PrivKey.Q = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "d":
                    Key.PrivKey.D = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "dmp1":
                    Key.PrivKey.DMP1 = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "dmq1":
                    Key.PrivKey.DMQ1 = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "iqmp":
                    Key.PrivKey.IQMP = new BitString(value).ToPositiveBigInteger();
                    return true;
                case "seed":
                    Seed = new BitString(value);
                    return true;
            }

            return false;
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

        private BigInteger BigIntegerFromObject(string sourcePropertyName, ExpandoObject source)
        {
            return 0;
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
