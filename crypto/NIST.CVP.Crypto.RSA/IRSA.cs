using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public interface IRSA
    {
        EncryptResult EncryptRSA(BigInteger message, KeyPair keyPair);
        DecryptResult DecryptRSA(BigInteger ciphertext, KeyPair keyPair);
    }
}
