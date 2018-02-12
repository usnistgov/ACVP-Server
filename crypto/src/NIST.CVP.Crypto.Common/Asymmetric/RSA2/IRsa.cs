using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public interface IRsa
    {
        EncryptionResult Encrypt(BigInteger plainText, PublicKey pubKey);
        DecryptionResult Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey);
    }
}
