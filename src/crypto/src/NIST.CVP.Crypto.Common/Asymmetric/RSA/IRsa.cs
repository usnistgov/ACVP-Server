using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA
{
    public interface IRsa
    {
        EncryptionResult Encrypt(BigInteger plainText, PublicKey pubKey);
        DecryptionResult Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey);
    }
}
