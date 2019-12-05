using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math.Helpers;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA
{
    public class RsaVisitor : IRsaVisitor
    {
        public BigInteger Decrypt(BigInteger cipherText, CrtPrivateKey privKey, PublicKey pubKey)
        {
            var m1 = BigInteger.ModPow(cipherText, privKey.DMP1, privKey.P);
            var m2 = BigInteger.ModPow(cipherText, privKey.DMQ1, privKey.Q);
            var h = (privKey.IQMP * (m1 - m2)).PosMod(privKey.P);
            return m2 + (h * privKey.Q);
        }

        public BigInteger Decrypt(BigInteger cipherText, PrivateKey privKey, PublicKey pubKey)
        {
            return BigInteger.ModPow(cipherText, privKey.D, pubKey.N);
        }
    }
}
