using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IPrimeGeneratorFactory
    {
        //IPrimeGenerator GetKatPrimeGenerator();
        IFips186_2PrimeGenerator GetFips186_2PrimeGenerator(IEntropyProvider entropyProvider, PrimeTestModes primeTest);
        IFips186_4PrimeGenerator GetFips186_4PrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest);
        IFips186_5PrimeGenerator GetFips186_5PrimeGenerator(PrimeGenModes primeGen, ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest);
    }
}
