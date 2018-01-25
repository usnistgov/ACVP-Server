using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.Keys;

namespace NIST.CVP.Crypto.RSA2
{
    public class CrtRsa : IRsa<CrtPrivateKey>
    {
        public BigInteger Encrypt(BigInteger plainText, PublicKey pubKey, CrtPrivateKey privKey)
        {
            throw new NotImplementedException();
        }

        public BigInteger Decrypt(BigInteger cipherText, PublicKey pubKey, CrtPrivateKey privKey)
        {
            throw new NotImplementedException();
        }
    }
}
