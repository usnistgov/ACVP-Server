using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Keys
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
