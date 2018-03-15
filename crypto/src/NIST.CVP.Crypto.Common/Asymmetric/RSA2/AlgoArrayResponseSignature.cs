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
        public KeyPair Key { get; set; } = new KeyPair() { PubKey = new PublicKey() };

        public BigInteger E
        {
            get => Key.PubKey.E;
            set => Key.PubKey.E = value;
        }

        public BigInteger N
        {
            get => Key.PubKey.N;
            set => Key.PubKey.N = value;
        }

        public bool FailureTest { get; set; }
    }
}
