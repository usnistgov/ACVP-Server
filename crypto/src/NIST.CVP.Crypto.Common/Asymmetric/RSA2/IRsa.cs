using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public interface IRsa
    {
        BigInteger Encrypt(BigInteger plainText, PublicKey pubKey);
        BigInteger Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey);
    }
}
