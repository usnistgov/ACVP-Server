using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys
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
        IKeyBuilder WithStandard(Fips186Standard standard);
        IKeyBuilder WithPMod8(int a);
        IKeyBuilder WithQMod8(int b);
    }
}
