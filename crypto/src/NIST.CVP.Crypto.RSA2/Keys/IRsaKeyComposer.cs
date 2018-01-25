using NIST.CVP.Crypto.RSA2.Keys;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public interface IRsaKeyComposer
    {
        KeyPair ComposeKey(BigInteger e, PrimePair primes);
    }
}
