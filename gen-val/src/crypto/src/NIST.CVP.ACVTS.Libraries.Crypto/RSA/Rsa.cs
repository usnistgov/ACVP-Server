using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA
{
    public class Rsa : IRsa
    {
        private readonly IRsaVisitor _visitor;

        public Rsa(IRsaVisitor visitor)
        {
            _visitor = visitor;
        }

        public EncryptionResult Encrypt(BigInteger plainText, PublicKey pubKey)
        {
            if (plainText <= 1 || plainText >= pubKey.N - 1)
            {
                return new EncryptionResult("Plaintext too long");
            }

            return new EncryptionResult(BigInteger.ModPow(plainText, pubKey.E, pubKey.N));
        }

        public DecryptionResult Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey)
        {
            if (cipherText <= 1 || cipherText >= pubKey.N - 1)
            {
                return new DecryptionResult("Ciphertext too long");
            }

            return new DecryptionResult(privKey.AcceptDecrypt(_visitor, cipherText, pubKey));
        }
    }
}
