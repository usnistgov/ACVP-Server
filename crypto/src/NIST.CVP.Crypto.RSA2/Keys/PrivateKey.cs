using System.Numerics;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class PrivateKey : IRsaPrivateKey
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }

        public BigInteger D { get; set; }
    }
}
