using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString Seed { get; set; }
        public int[] Bitlens { get; set; }

        // Potential auxiliary values
        public BitString XP { get; set; } = new BitString(0);
        public BitString XQ { get; set; } = new BitString(0);

        public BitString XP1 { get; set; } = new BitString(0);
        public BitString XP2 { get; set; } = new BitString(0);
        public BitString XQ1 { get; set; } = new BitString(0);
        public BitString XQ2 { get; set; } = new BitString(0);

        [JsonIgnore]
        public KeyPair Key { get; set; } = new KeyPair { PubKey = new PublicKey() };

        #region Key Value Getters and Setters
        public BigInteger N
        {
            get => Key.PubKey.N;
            set => Key.PubKey.N = value;
        }

        public BigInteger E
        {
            get => Key.PubKey.E;
            set => Key.PubKey.E = value;
        }

        public BigInteger P
        {
            get => Key.PrivKey.P;
            set => Key.PrivKey.P = value;
        }

        public BigInteger Q
        {
            get => Key.PrivKey.Q;
            set => Key.PrivKey.Q = value;
        }

        private BigInteger _d;
        public BigInteger D
        {
            get
            {
                if (Key.PrivKey is PrivateKey privKey)
                {
                    return privKey.D;
                }

                return 0;
            }

            set
            {
                _d = value;
                if (_d != 0)
                {
                    Key.PrivKey = new PrivateKey
                    {
                        D = _d,
                        P = P,
                        Q = Q
                    };
                }
            }
        }

        private BigInteger _dmp1;
        public BigInteger Dmp1
        {
            get
            {
                if (Key.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.DMP1;
                }

                return 0;
            }

            set
            {
                _dmp1 = value;
                if (_dmp1 != 0)
                {
                    Key.PrivKey = new CrtPrivateKey
                    {
                        P = P,
                        Q = Q,
                        DMP1 = _dmp1,
                        DMQ1 = Dmq1,
                        IQMP = Iqmp
                    };
                }
            }
        }

        private BigInteger _dmq1;
        public BigInteger Dmq1
        {
            get
            {
                if (Key.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.DMQ1;
                }

                return 0;
            }

            set
            {
                _dmq1 = value;
                if (_dmq1 != 0)
                {
                    Key.PrivKey = new CrtPrivateKey
                    {
                        P = P,
                        Q = Q,
                        DMP1 = Dmp1,
                        DMQ1 = _dmq1,
                        IQMP = Iqmp
                    };
                }
            }
        }

        private BigInteger _iqmp;
        public BigInteger Iqmp
        {
            get
            {
                if (Key.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.IQMP;
                }

                return 0;
            }

            set
            {
                _iqmp = value;
                if (_iqmp != 0)
                {
                    Key.PrivKey = new CrtPrivateKey
                    {
                        P = P,
                        Q = Q,
                        DMP1 = Dmp1,
                        DMQ1 = Dmq1,
                        IQMP = _iqmp
                    };
                }
            }
        }
        #endregion Key Value Getters and Setters

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
                    P = new BitString(value).ToPositiveBigInteger();
                    return true;
                
                case "q":
                    Q = new BitString(value).ToPositiveBigInteger();
                    return true;
                
                case "d":
                    // Assume that D is the last value of the private key
                    Key.PrivKey = new PrivateKey
                    {
                        D = new BitString(value).ToPositiveBigInteger(),
                        P = P,
                        Q = Q
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
    }
}
