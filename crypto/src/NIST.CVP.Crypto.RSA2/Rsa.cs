using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
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

        public BigInteger Encrypt(BigInteger plainText, PublicKey pubKey)
        {
            return BigInteger.ModPow(plainText, pubKey.E, pubKey.N);
        }

        public BigInteger Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey)
        {
            return privKey.AcceptDecrypt(_visitor, cipherText, pubKey);
        }
    }
}
