using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public interface IKeyBuilder
    {
        KeyResult Build();
        IKeyBuilder WithBitlens(int[] bitlens);
        IKeyBuilder WithEntropyProvider(IEntropyProvider entropyProvider);
        IKeyBuilder WithHashFunction(ISha sha);
        IKeyBuilder WithKeyComposer(IRsaKeyComposer keyComposer);
        IKeyBuilder WithNlen(int nlen);
        IKeyBuilder WithPrimeGenMode(PrimeGenModes primeGenMode);
        IKeyBuilder WithPrimeTestMode(PrimeTestModes primeTestMode);
        IKeyBuilder WithPublicExponent(BigInteger e);
        IKeyBuilder WithPublicExponent(BitString e);
        IKeyBuilder WithSeed(BitString seed);
    }
}