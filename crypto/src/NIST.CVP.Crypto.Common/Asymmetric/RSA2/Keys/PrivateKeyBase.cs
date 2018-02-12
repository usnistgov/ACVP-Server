using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public abstract class PrivateKeyBase : IRsaVisitable, IRsaPrivateKey
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }

        public abstract BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey);
    }
}
