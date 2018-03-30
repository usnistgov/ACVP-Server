using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed);
    }
}
