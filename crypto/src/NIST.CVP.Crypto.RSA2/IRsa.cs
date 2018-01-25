using System.Numerics;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public interface IRsa<in TPrivateKey>
        where TPrivateKey : IRsaPrivateKey
    {
        BigInteger Encrypt(BigInteger plainText, PublicKey pubKey, TPrivateKey privKey);
        BigInteger Decrypt(BigInteger cipherText, PublicKey pubKey, TPrivateKey privKey);
    }
}
