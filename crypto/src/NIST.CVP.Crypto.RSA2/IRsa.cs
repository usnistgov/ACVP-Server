using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public interface IRsa
    {
        BigInteger Encrypt(BigInteger plainText, PublicKey pubKey);
        BigInteger Decrypt(BigInteger cipherText, PrivateKeyBase privKey, PublicKey pubKey);
    }
}
