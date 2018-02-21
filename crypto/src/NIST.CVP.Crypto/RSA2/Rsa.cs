using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
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
            // For SP-Component mainly, but this shouldn't really happen anywhere else
            // TODO does null check on pub key make sense?  Was getting NRE when pubKey null
            if (cipherText <= 1 || cipherText >= pubKey.N - 1)
            {
                return new DecryptionResult("Ciphertext too long");
            }

            return new DecryptionResult(privKey.AcceptDecrypt(_visitor, cipherText, pubKey));
        }
    }
}
