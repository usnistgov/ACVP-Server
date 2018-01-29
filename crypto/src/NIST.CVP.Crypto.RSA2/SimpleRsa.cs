using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public class SimpleRsa : IRsa<PrivateKey>
    {
        public BigInteger Encrypt(BigInteger plainText, PublicKey pubKey)
        {
            return BigInteger.ModPow(plainText, pubKey.E, pubKey.N);
        }

        public BigInteger Decrypt(BigInteger cipherText, PublicKey pubKey, PrivateKey privKey)
        {
            return BigInteger.ModPow(cipherText, privKey.D, pubKey.N);
        }
    }
}
