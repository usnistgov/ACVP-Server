using System.Numerics;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IPrimeGeneratorBase : IPrimeGenerator
    {
        void AddEntropy(BigInteger entropy);
        void AddEntropy(BitString entropy);
        PPFResult ProbablePrimeFactor(BigInteger r1, BigInteger r2, int nlen, BigInteger e, int security_strength);
        PPCResult ProvablePrimeConstruction(int L, int N1, int N2, BigInteger firstSeed, BigInteger e);
        void SetBitlens(int b1, int b2, int b3, int b4);
        void SetBitlens(int[] bitlens);
        void SetEntropyProviderType(EntropyProviderTypes type);
        void SetHashFunction(HashFunction hashFunction);
        void SetPrimeTestMode(PrimeTestModes ptMode);
    }
}