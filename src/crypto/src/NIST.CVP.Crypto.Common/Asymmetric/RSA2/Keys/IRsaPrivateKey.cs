using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public interface IRsaPrivateKey
    {
        BigInteger P { get; set; }
        BigInteger Q { get; set; }
    }
}
