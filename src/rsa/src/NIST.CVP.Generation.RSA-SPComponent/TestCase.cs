using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        [JsonIgnore]
        public KeyPair Key { get; set; } = new KeyPair {PubKey = new PublicKey()};
        public BitString Message { get; set; }
        public BitString Signature { get; set; }

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

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "em":
                    Message = new BitString(value);
                    return true;

                case "s":
                    if (value.Contains("fail"))
                    {
                        TestPassed = false;
                        return true;
                    }
                    Signature = new BitString(value);
                    TestPassed = true;
                    return true;

                case "n":
                    Key = new KeyPair {PrivKey = new PrivateKey(), PubKey = new PublicKey()};
                    Key.PubKey.N = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "p":
                    Key.PrivKey.P = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "q":
                    Key.PrivKey.Q = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "d":
                    ((PrivateKey) Key.PrivKey).D = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "e":
                    Key.PubKey.E = new BitString(value).ToPositiveBigInteger();
                    return true;
            }

            return false;
        }
    }
}
