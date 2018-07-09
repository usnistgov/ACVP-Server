using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    // Chinese Remainder Theorem
    public class CrtPrivateKey : PrivateKeyBase
    {
        public BigInteger DMP1 { get; set; }
        public BigInteger DMQ1 { get; set; }
        public BigInteger IQMP { get; set; }

        public override BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey)
        {
            return visitor.Decrypt(cipherText, this, pubKey);
        }
    }
}
