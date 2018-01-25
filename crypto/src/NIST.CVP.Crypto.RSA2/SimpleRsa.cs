using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public class SimpleRsa : IRsa<PrivateKey>
    {
        public BigInteger Encrypt(BigInteger plainText, PublicKey pubKey, PrivateKey privKey)
        {
            throw new NotImplementedException();
        }

        public BigInteger Decrypt(BigInteger cipherText, PublicKey pubKey, PrivateKey privKey)
        {
            throw new NotImplementedException();
        }
    }
}
