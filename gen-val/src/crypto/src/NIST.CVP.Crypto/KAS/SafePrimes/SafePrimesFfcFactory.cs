using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.SafePrimes;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.SafePrimes
{
    public class SafePrimesFfcFactory : ISafePrimesFfcFactory
    {
        public IDsaFfc GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new SafePrimesFfc();
        }
    }
}