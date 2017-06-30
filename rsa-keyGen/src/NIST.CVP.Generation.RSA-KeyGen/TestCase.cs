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

        // Potential auxiliary values
        public BigInteger XP1 { get; set; }
        public BigInteger XP2 { get; set; }
        public BigInteger XP { get; set; }
        public BigInteger XQ1 { get; set; }
        public BigInteger XQ2 { get; set; }
        public BigInteger XQ { get; set; }

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

            // Nothing to merge here
            if (Key == null || otherTest.Deferred)
            {
                return true;
            }
            
            var otherTypedTest = (TestCase) otherTest;
            var retVal = false;

            if (XP == 0 && otherTypedTest.XP != 0)
            {
                XP = otherTypedTest.XP;
                XQ = otherTypedTest.XQ;
                retVal = true;
            }

            if (XP1 == 0 && otherTypedTest.XP1 != 0)
            {
                XP1 = otherTypedTest.XP1;
                XP2 = otherTypedTest.XP2;
                XQ1 = otherTypedTest.XQ1;
                XQ2 = otherTypedTest.XQ2;
                retVal = true;
            }

            if (Key.PubKey.E == 0 && otherTypedTest.Key.PubKey.E != 0)
            {
                Key.PubKey.E = otherTypedTest.Key.PubKey.E;
                retVal = true;
            }

            if (Seed == null && otherTypedTest.Seed != null)
            {
                Seed = otherTypedTest.Seed;
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

            if (((ExpandoObject) source).ContainsProperty("seed"))
            {
                Seed = BitStringFromObject("seed", (ExpandoObject)source);
            }

            if (((ExpandoObject) source).ContainsProperty("bitlens"))
            {
                Bitlens = IntArrayFromObject("bitlens", source);
            }

            if (((ExpandoObject) source).ContainsProperty("e"))
            {
                Key = new KeyPair {PubKey = new PublicKey {E = new BitString(source.e).ToPositiveBigInteger()}};
            }

            if (((ExpandoObject) source).ContainsProperty("p"))
            {
                Key = KeyPairFromObject((ExpandoObject)source);
            }

            if (((ExpandoObject) source).ContainsProperty("result"))
            {
                FailureTest = source.result == "failed";
            }

            if (((ExpandoObject) source).ContainsProperty("xp"))
            {
                XP = BigIntegerFromObject("xp", source);
                XQ = BigIntegerFromObject("xq", source);
            }

            if (((ExpandoObject) source).ContainsProperty("xp1"))
            {
                XP1 = BigIntegerFromObject("xp1", source);
                XP2 = BigIntegerFromObject("xp2", source);
                XQ1 = BigIntegerFromObject("xq1", source);
                XQ2 = BigIntegerFromObject("xq2", source);
            }
        }

        private KeyPair KeyPairFromObject(ExpandoObject source)
        {
            var e = BitStringFromObject("e", source);
            var p = BitStringFromObject("p", source);
            var q = BitStringFromObject("q", source);
            var n = BitStringFromObject("n", source);

            var d = BitStringFromObject("d", source);
            var dmp1 = BitStringFromObject("dmp1", source);
            var dmq1 = BitStringFromObject("dmq1", source);
            var iqmp = BitStringFromObject("iqmp", source);

            if (d == null)
            {
                return new KeyPair(p, q, n, e, dmp1, dmq1, iqmp);
            }
            else
            {
                return new KeyPair(p, q, n, e, d);
            }
        }

        private int[] IntArrayFromObject(string sourcePropertyName, ExpandoObject source)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];

            var valueAsList = sourcePropertyValue as List<int>;
            if (valueAsList?.Count != 4)
            {
                return null;
            }

            return valueAsList.ToArray();
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

            return new BitString(sourcePropertyValue.ToString()).ToPositiveBigInteger();
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
