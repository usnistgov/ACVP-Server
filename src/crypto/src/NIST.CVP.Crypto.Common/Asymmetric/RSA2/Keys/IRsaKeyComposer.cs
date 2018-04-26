using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public interface IRsaKeyComposer
    {
        KeyPair ComposeKey(BigInteger e, PrimePair primes);
    }
}
