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
        public BitString XP1 { get; set; }
        public BitString XP2 { get; set; }
        public BitString XP { get; set; }
        public BitString XQ1 { get; set; }
        public BitString XQ2 { get; set; }
        public BitString XQ { get; set; }

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

            if (XP == null && otherTypedTest.XP != null)
            {
                XP = otherTypedTest.XP.GetDeepCopy();
                XQ = otherTypedTest.XQ.GetDeepCopy();
                retVal = true;
            }

            if (XP1 == null && otherTypedTest.XP1 != null)
            {
                XP1 = otherTypedTest.XP1.GetDeepCopy();
                XP2 = otherTypedTest.XP2.GetDeepCopy();
                XQ1 = otherTypedTest.XQ1.GetDeepCopy();
                XQ2 = otherTypedTest.XQ2.GetDeepCopy();
                retVal = true;
            }

            if (Key.PubKey.E == 0 && otherTypedTest.Key.PubKey.E != 0)
            {
                Key.PubKey.E = otherTypedTest.Key.PubKey.E;
                retVal = true;
            }

            if (Seed == null && otherTypedTest.Seed != null)
            {
                Seed = otherTypedTest.Seed.GetDeepCopy();
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
                    if (Key == null) { Key = new KeyPair(); }
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
                case "seed":
                    Seed = new BitString(value);
                    return true;
                case "bitlen1":
                    Bitlens = new int[4];
                    Bitlens[0] = int.Parse(value);
                    return true;
                case "bitlen2":
                    Bitlens[1] = int.Parse(value);
                    return true;
                case "bitlen3":
                    Bitlens[2] = int.Parse(value);
                    return true;
                case "bitlen4":
                    Bitlens[3] = int.Parse(value);
                    return true;
                case "xp":
                    XP = new BitString(value);
                    return true;
                case "xp1":
                    XP1 = new BitString(value, Bitlens[0]);
                    return true;
                case "xp2":
                    XP2 = new BitString(value, Bitlens[1]);
                    return true;
                case "xq":
                    XQ = new BitString(value);
                    return true;
                case "xq1":
                    XQ1 = new BitString(value, Bitlens[2]);
                    return true;
                case "xq2":
                    XQ2 = new BitString(value, Bitlens[3]);
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
                XP = BitStringFromObject("xp", source);
                XQ = BitStringFromObject("xq", source);
            }

            if (((ExpandoObject) source).ContainsProperty("xp1"))
            {
                XP1 = ((BitString)BitStringFromObject("xp1", source)).Substring(0, Bitlens[0]);
                XP2 = ((BitString)BitStringFromObject("xp2", source)).Substring(0, Bitlens[1]);
                XQ1 = ((BitString)BitStringFromObject("xq1", source)).Substring(0, Bitlens[2]);
                XQ2 = ((BitString)BitStringFromObject("xq2", source)).Substring(0, Bitlens[3]);
            }
        }

        private KeyPair KeyPairFromObject(ExpandoObject source)
        {
            var e = BigIntegerFromObject("e", source);
            var p = BigIntegerFromObject("p", source);
            var q = BigIntegerFromObject("q", source);
            var n = BigIntegerFromObject("n", source);

            var d = BigIntegerFromObject("d", source);
            var dmp1 = BigIntegerFromObject("dmp1", source);
            var dmq1 = BigIntegerFromObject("dmq1", source);
            var iqmp = BigIntegerFromObject("iqmp", source);

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

            var valueAsArr = ((List<object>)sourcePropertyValue).ToArray();
            if (valueAsArr.Count() != 4)
            {
                return null;
            }

            var intArr = new int[4];
            for(var i = 0; i < valueAsArr.Count(); i++)
            {
                intArr[i] = (int) (long) valueAsArr[i];
            }

            return intArr;
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

            if(sourcePropertyValue.GetType() == typeof(string))
            {
                return new BitString(sourcePropertyValue.ToString()).ToPositiveBigInteger();
            }

            var valueAsBigInteger = (BigInteger)sourcePropertyValue;
            if(valueAsBigInteger != 0)
            {
                return valueAsBigInteger;
            }

            return 0;
        }

        private BitString BitStringFromObject(string sourcePropertyName, ExpandoObject source, int length = -1)
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

            return new BitString(sourcePropertyValue.ToString(), length);
        }
    }
}
