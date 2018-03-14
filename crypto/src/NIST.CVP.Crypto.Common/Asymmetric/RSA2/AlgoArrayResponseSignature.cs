using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class AlgoArrayResponseSignature
    {
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        
        [JsonIgnore]
        public KeyPair Key { get; set; }

        public BigInteger E
        {
            get
            {
                if (Key == null) return 0;
                return Key.PubKey.E;
            }

            set
            {
                if (Key == null)
                {
                    Key = new KeyPair
                    {
                        PubKey = new PublicKey
                        {
                            E = value
                        }
                    };
                }
                else
                {
                    Key.PubKey.E = value;
                }
            }
        }

        public BigInteger N
        {
            get
            {
                if (Key == null) return 0;
                return Key.PubKey.N;
            }

            set
            {
                if (Key == null)
                {
                    Key = new KeyPair
                    {
                        PubKey = new PublicKey
                        {
                            N = value
                        }
                    };
                }
                else
                {
                    Key.PubKey.N = value;
                }
            }
        }

        public bool FailureTest { get; set; }
    }
}
