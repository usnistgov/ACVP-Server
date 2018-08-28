using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public class PrivateKey : PrivateKeyBase
    {
        public override BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey)
        {
            return visitor.Decrypt(cipherText, this, pubKey);
        }
    }
}
