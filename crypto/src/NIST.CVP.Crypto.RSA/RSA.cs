using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class RSA : IRSA
    {
        public EncryptResult EncryptRSA(BigInteger message, KeyPair keyPair)
        {
            throw new NotImplementedException();
        }

        public DecryptResult DecryptRSA(BigInteger ciphertext, KeyPair keyPair)
        {
            throw new NotImplementedException();
        }

        public SignatureResult SignRSA(BigInteger message, KeyPair keyPair)
        {
            throw new NotImplementedException();
        }

        public VerifyResult VerifyRSA(BigInteger signature, KeyPair keyPair)
        {
            throw new NotImplementedException();
        }
    }
}
