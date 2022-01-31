using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA
{
    public interface IRsaVisitor
    {
        BigInteger Decrypt(BigInteger cipherText, CrtPrivateKey privKey, PublicKey pubKey);
        BigInteger Decrypt(BigInteger cipherText, PrivateKey privKey, PublicKey pubKey);
    }
}
