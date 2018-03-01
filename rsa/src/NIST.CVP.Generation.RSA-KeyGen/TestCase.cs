using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
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

        public ITestGroup Parent { get; set; }

        private BigInteger _p;
        private BigInteger _q;

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

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "e":
                    // Assume that E is the first value of the key
                    if (Key == null)
                    {
                        Key = new KeyPair
                        {
                            PrivKey = new PrivateKey(),
                            PubKey = new PublicKey()
                        };
                    }
                    Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "n":
                    Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;
                
                case "p":
                    _p = new BitString(value).ToPositiveBigInteger();
                    return true;
                
                case "q":
                    _q = new BitString(value).ToPositiveBigInteger();
                    return true;
                
                case "d":
                    // Assume that D is the last value of the private key
                    Key.PrivKey = new PrivateKey
                    {
                        D = new BitString(value).ToPositiveBigInteger(),
                        P = _p,
                        Q = _q
                    };
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
                
                case "xq":
                    XQ = new BitString(value);
                    return true;
                
                // Some hard assumptions made here. These values are used only for B.3.6 now.
                // These values need the bitlen values, but as hex are represented as integers
                // So we need the least significant bits from the full hex string, not truncating off
                // the back like the constructor would with the int value.
                case "xp1":
                    XP1 = new BitString(value);
                    XP1 = XP1.GetLeastSignificantBits(Bitlens[0]);
                    return true;
                
                case "xp2":
                    XP2 = new BitString(value);
                    XP2 = XP2.GetLeastSignificantBits(Bitlens[1]);
                    return true;
                
                case "xq1":
                    XQ1 = new BitString(value);
                    XQ1 = XQ1.GetLeastSignificantBits(Bitlens[2]);
                    return true;
                
                case "xq2":
                    XQ2 = new BitString(value);
                    XQ2 = XQ2.GetLeastSignificantBits(Bitlens[3]);
                    return true;
            }

            return false;
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int) source.tcId;
            var expandoSource = (ExpandoObject) source;

            Deferred = expandoSource.GetTypeFromProperty<bool>("deferred");
            FailureTest = expandoSource.GetTypeFromProperty<bool>("result");

            //Bitlens = expandoSource.GetTypeFromProperty<int[]>("bitlens");
            Bitlens = IntArrayFromObject("bitlens", expandoSource);
            Key = KeyPairFromObject(expandoSource);

            Seed = expandoSource.GetBitStringFromProperty("seed");

            XP = expandoSource.GetBitStringFromProperty("xp");
            XQ = expandoSource.GetBitStringFromProperty("xq");
            XP1 = expandoSource.GetBitStringFromProperty("xp1");
            XQ1 = expandoSource.GetBitStringFromProperty("xq1");
            XP2 = expandoSource.GetBitStringFromProperty("xp2");
            XQ2 = expandoSource.GetBitStringFromProperty("xq2");

            // TODO Some hard assumptions being made all up in here
            if (Bitlens != null && XP1 != null)
            {
                XP1 = XP1.GetLeastSignificantBits(Bitlens[0]);
                XP2 = XP2.GetLeastSignificantBits(Bitlens[1]);
                XQ1 = XQ1.GetLeastSignificantBits(Bitlens[2]);
                XQ2 = XQ2.GetLeastSignificantBits(Bitlens[3]);
            }
        }

        private KeyPair KeyPairFromObject(ExpandoObject source)
        {
            var e = source.GetBigIntegerFromProperty("e");
            var n = source.GetBigIntegerFromProperty("n");
            
            var p = source.GetBigIntegerFromProperty("p");
            var q = source.GetBigIntegerFromProperty("q");

            var d = source.GetBigIntegerFromProperty("d");
            var dmp1 = source.GetBigIntegerFromProperty("dmp1");
            var dmq1 = source.GetBigIntegerFromProperty("dmq1");
            var iqmp = source.GetBigIntegerFromProperty("iqmp");
            
            var pubKey = new PublicKey
            {
                N = n,
                E = e
            };

            PrivateKeyBase privKey;
            if (d == 0)
            {
                privKey = new CrtPrivateKey
                {
                    DMP1 = dmp1,
                    DMQ1 = dmq1,
                    IQMP = iqmp,
                    P = p,
                    Q = q
                };
            }
            else
            {
                privKey = new PrivateKey
                {
                    P = p,
                    Q = q,
                    D = d
                };
            }

            return new KeyPair {PrivKey = privKey, PubKey = pubKey};
        }

        private int[] IntArrayFromObject(string sourcePropertyName, ExpandoObject source)
        {
            if (!source.ContainsProperty(sourcePropertyName))
            {
                return null;
            }

            var sourcePropertyValue = ((IDictionary<string, object>)source)[sourcePropertyName];

            if (sourcePropertyValue is int[] val)
            {
                return val.Length == 4 ? val : null;
            }

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
    }
}
