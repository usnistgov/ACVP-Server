using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public interface IRsaVisitable
    {
        BigInteger AcceptDecrypt(IRsaVisitor visitor, BigInteger cipherText, PublicKey pubKey);
    }
}
