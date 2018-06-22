using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public interface IRsaVisitor
    {
        BigInteger Decrypt(BigInteger cipherText, CrtPrivateKey privKey, PublicKey pubKey);
        BigInteger Decrypt(BigInteger cipherText, PrivateKey privKey, PublicKey pubKey);
    }
}
