using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public interface IRsaVisitor
    {
        BigInteger Decrypt(BigInteger cipherText, CrtPrivateKey privKey, PublicKey pubKey);
        BigInteger Decrypt(BigInteger cipherText, PrivateKey privKey, PublicKey pubKey);
    }
}
