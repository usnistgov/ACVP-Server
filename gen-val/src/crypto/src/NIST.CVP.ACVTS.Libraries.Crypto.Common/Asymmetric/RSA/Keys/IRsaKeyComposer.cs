using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys
{
    public interface IRsaKeyComposer
    {
        KeyPair ComposeKey(BigInteger e, PrimePair primes);
    }
}
