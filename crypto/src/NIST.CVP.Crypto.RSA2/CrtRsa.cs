using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2
{
    public class CrtRsa : IRsa<CrtPrivateKey>
    {
        public BigInteger Encrypt(BigInteger plainText, PublicKey pubKey)
        {
            return BigInteger.ModPow(plainText, pubKey.E, pubKey.N);
        }

        public BigInteger Decrypt(BigInteger cipherText, PublicKey pubKey, CrtPrivateKey privKey)
        {
            var m1 = BigInteger.ModPow(cipherText, privKey.DMP1, privKey.P);
            var m2 = BigInteger.ModPow(cipherText, privKey.DMQ1, privKey.Q);
            var h = (privKey.IQMP * (m1 - m2)).PosMod(privKey.P);
            return m2 + (h * privKey.Q);
        }
    }
}
