using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public interface IRsaKeyComposer
    {
        KeyPair ComposeKey(BigInteger e, PrimePair primes);
    }
}
