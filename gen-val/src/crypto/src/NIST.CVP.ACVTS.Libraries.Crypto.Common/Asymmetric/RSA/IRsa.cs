using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA
{
    public interface IRsa
    {
        EncryptionResult Encrypt(BigInteger plainText, PublicKey pubKey);
        DecryptionResult Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey);
    }
}
