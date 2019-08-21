using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; }
        public bool Deferred { get; }
        
        public BitString ServerId { get; set; }
        public BitString IutId { get; set; }
        
        [JsonIgnore]
        public KeyPair ServerKey { get; set; } = new KeyPair { PubKey = new PublicKey() };

        #region Server Key Value Getters and Setters
        public BitString ServerN
        {
            get => ServerKey.PubKey.N != 0 ? new BitString(ServerKey.PubKey.N, ParentGroup?.Modulo ?? 0) : null;
            set => ServerKey.PubKey.N = value.ToPositiveBigInteger();
        }

        public BigInteger ServerE
        {
            get => ServerKey.PubKey.E;
            set => ServerKey.PubKey.E = value;
        }

        public BigInteger ServerP
        {
            get => ServerKey.PrivKey.P;
            set => ServerKey.PrivKey.P = value;
        }

        public BigInteger ServerQ
        {
            get => ServerKey.PrivKey.Q;
            set => ServerKey.PrivKey.Q = value;
        }

        private BigInteger _serverD;
        public BigInteger ServerD
        {
            get
            {
                if (ServerKey.PrivKey is PrivateKey privKey)
                {
                    return privKey.D;
                }

                return 0;
            }

            set
            {
                _serverD = value;
                if (_serverD != 0)
                {
                    ServerKey.PrivKey = new PrivateKey
                    {
                        D = _serverD,
                        P = ServerP,
                        Q = ServerQ
                    };
                }
            }
        }

        private BigInteger _serverDmp1;
        public BigInteger ServerDmp1
        {
            get
            {
                if (ServerKey.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.DMP1;
                }

                return 0;
            }

            set
            {
                _serverDmp1 = value;
                if (_serverDmp1 != 0)
                {
                    ServerKey.PrivKey = new CrtPrivateKey
                    {
                        P = ServerP,
                        Q = ServerQ,
                        DMP1 = _serverDmp1,
                        DMQ1 = ServerDmq1,
                        IQMP = ServerIqmp
                    };
                }
            }
        }

        private BigInteger _serverDmq1;
        public BigInteger ServerDmq1
        {
            get
            {
                if (ServerKey.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.DMQ1;
                }

                return 0;
            }

            set
            {
                _serverDmq1 = value;
                if (_serverDmq1 != 0)
                {
                    ServerKey.PrivKey = new CrtPrivateKey
                    {
                        P = ServerP,
                        Q = ServerQ,
                        DMP1 = ServerDmp1,
                        DMQ1 = _serverDmq1,
                        IQMP = ServerIqmp
                    };
                }
            }
        }

        private BigInteger _serverIqmp;
        public BigInteger ServerIqmp
        {
            get
            {
                if (ServerKey.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.IQMP;
                }

                return 0;
            }

            set
            {
                _serverIqmp = value;
                if (_serverIqmp != 0)
                {
                    ServerKey.PrivKey = new CrtPrivateKey
                    {
                        P = ServerP,
                        Q = ServerQ,
                        DMP1 = ServerDmp1,
                        DMQ1 = ServerDmq1,
                        IQMP = _serverIqmp
                    };
                }
            }
        }
        #endregion Server Key Value Getters and Setters
        
        [JsonIgnore]
        public KeyPair IutKey { get; set; } = new KeyPair { PubKey = new PublicKey() };

        #region Iut Key Value Getters and Setters
        public BitString IutN
        {
            get => IutKey.PubKey.N != 0 ? new BitString(IutKey.PubKey.N, ParentGroup?.Modulo ?? 0) : null;
            set => IutKey.PubKey.N = value.ToPositiveBigInteger();
        }

        public BigInteger IutE
        {
            get => IutKey.PubKey.E;
            set => IutKey.PubKey.E = value;
        }

        public BigInteger IutP
        {
            get => IutKey.PrivKey.P;
            set => IutKey.PrivKey.P = value;
        }

        public BigInteger IutQ
        {
            get => IutKey.PrivKey.Q;
            set => IutKey.PrivKey.Q = value;
        }

        private BigInteger _iutD;
        public BigInteger IutD
        {
            get
            {
                if (IutKey.PrivKey is PrivateKey privKey)
                {
                    return privKey.D;
                }

                return 0;
            }

            set
            {
                _iutD = value;
                if (_iutD != 0)
                {
                    IutKey.PrivKey = new PrivateKey
                    {
                        D = _iutD,
                        P = IutP,
                        Q = IutQ
                    };
                }
            }
        }

        private BigInteger _iutDmp1;
        public BigInteger IutDmp1
        {
            get
            {
                if (IutKey.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.DMP1;
                }

                return 0;
            }

            set
            {
                _iutDmp1 = value;
                if (_iutDmp1 != 0)
                {
                    IutKey.PrivKey = new CrtPrivateKey
                    {
                        P = IutP,
                        Q = IutQ,
                        DMP1 = _iutDmp1,
                        DMQ1 = IutDmq1,
                        IQMP = IutIqmp
                    };
                }
            }
        }

        private BigInteger _iutDmq1;
        public BigInteger IutDmq1
        {
            get
            {
                if (IutKey.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.DMQ1;
                }

                return 0;
            }

            set
            {
                _iutDmq1 = value;
                if (_iutDmq1 != 0)
                {
                    IutKey.PrivKey = new CrtPrivateKey
                    {
                        P = IutP,
                        Q = IutQ,
                        DMP1 = IutDmp1,
                        DMQ1 = _iutDmq1,
                        IQMP = IutIqmp
                    };
                }
            }
        }

        private BigInteger _iutIqmp;
        public BigInteger IutIqmp
        {
            get
            {
                if (IutKey.PrivKey is CrtPrivateKey crtKey)
                {
                    return crtKey.IQMP;
                }

                return 0;
            }

            set
            {
                _iutIqmp = value;
                if (_iutIqmp != 0)
                {
                    IutKey.PrivKey = new CrtPrivateKey
                    {
                        P = IutP,
                        Q = IutQ,
                        DMP1 = IutDmp1,
                        DMQ1 = IutDmq1,
                        IQMP = _iutIqmp
                    };
                }
            }
        }
        #endregion Iut Key Value Getters and Setters
        
        public BitString KdfNonce { get; set; }
        
        public BitString FixedInfo { get; set; }
        
        public BitString Z { get; set; }
        public BitString Key { get; set; }
        public BitString Tag { get; set; }
    }
}