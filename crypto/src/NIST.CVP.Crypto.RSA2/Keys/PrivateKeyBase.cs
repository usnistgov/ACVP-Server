using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public abstract class PrivateKeyBase : IRsaVisitable, IRsaPrivateKey
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }

        public abstract BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey);
    }
}
