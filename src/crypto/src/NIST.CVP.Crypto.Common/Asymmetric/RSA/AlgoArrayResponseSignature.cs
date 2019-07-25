using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA
{
    public class AlgoArrayResponseSignature
    {
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        
        /// <summary>
        /// This is used for serialization purposes within a sample of DP component in case of N values that when
        /// represented as a <see cref="BigInteger"/> are smaller than the modulo length.
        /// </summary>
        [JsonIgnore]
        public int Modulo { get; set; }
        
        [JsonIgnore]
        public KeyPair Key { get; set; } = new KeyPair() { PubKey = new PublicKey() };

        public BigInteger E
        {
            get => Key.PubKey.E;
            set => Key.PubKey.E = value;
        }

        public BitString N
        {
            get => new BitString(Key.PubKey.N, Modulo);
            set => Key.PubKey.N = value.ToPositiveBigInteger();
        }

        public bool TestPassed { get; set; }
    }
}
