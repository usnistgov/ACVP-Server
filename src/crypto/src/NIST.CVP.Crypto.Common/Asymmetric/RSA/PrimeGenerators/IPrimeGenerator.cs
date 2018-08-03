using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed);
        void SetBitlens(int[] bitlens);
    }
}
