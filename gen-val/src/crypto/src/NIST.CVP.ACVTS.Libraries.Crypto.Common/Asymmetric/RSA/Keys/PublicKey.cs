using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys
{
    public class PublicKey
    {
        public BigInteger E { get; set; }
        public BigInteger N { get; set; }
    }
}
