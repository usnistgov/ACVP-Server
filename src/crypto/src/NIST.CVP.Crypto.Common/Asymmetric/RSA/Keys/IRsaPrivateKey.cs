using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public interface IRsaPrivateKey
    {
        BigInteger P { get; set; }
        BigInteger Q { get; set; }
    }
}
