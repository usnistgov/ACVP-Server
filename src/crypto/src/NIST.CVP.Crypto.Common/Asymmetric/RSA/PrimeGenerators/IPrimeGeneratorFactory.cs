using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IPrimeGeneratorFactory
    {
        IPrimeGenerator GetPrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest);
    }
}
