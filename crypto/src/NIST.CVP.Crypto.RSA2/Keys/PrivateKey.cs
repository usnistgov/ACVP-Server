using System.Numerics;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public class PrivateKey : PrivateKeyBase
    {
        public BigInteger D { get; set; }

        public override BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey)
        {
            return visitor.Decrypt(cipherText, this, pubKey);
        }
    }
}
