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
            // For SP-Component mainly, but this shouldn't really happen anywhere else
            // TODO does null check on pub key make sense?  Was getting NRE when pubKey null
            if (cipherText >= pubKey?.N)
            {
                return 0;
            }

            return privKey.AcceptDecrypt(_visitor, cipherText, pubKey);
        }
    }
}
