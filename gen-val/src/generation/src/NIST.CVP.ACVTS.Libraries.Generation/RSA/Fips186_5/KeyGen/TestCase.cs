using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.KeyGen
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
        [JsonProperty(PropertyName = "xP")]
        public BitString XP { get; set; }
        [JsonProperty(PropertyName = "xQ")]
        public BitString XQ { get; set; }

        [JsonProperty(PropertyName = "xP1")]
        public BitString XP1 { get; set; }
        [JsonProperty(PropertyName = "xP2")]
        public BitString XP2 { get; set; }
        [JsonProperty(PropertyName = "xQ1")]
        public BitString XQ1 { get; set; }
        [JsonProperty(PropertyName = "xQ2")]
        public BitString XQ2 { get; set; }

        [JsonIgnore]
        public KeyPair Key { get; set; } = new KeyPair
        {
            PubKey = new PublicKey()
        };

        #region Key Value Getters and Setters
        public BitString N
        {
            get => Key.PubKey.N != 0 ? new BitString(Key.PubKey.N, ParentGroup?.Modulo ?? 0) : null;
            set => Key.PubKey.N = value.ToPositiveBigInteger();
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
        [JsonProperty(PropertyName = "dmp1")]
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
        [JsonProperty(PropertyName = "dmq1")]
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
        [JsonProperty(PropertyName = "iqmp")]
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
    }
}
