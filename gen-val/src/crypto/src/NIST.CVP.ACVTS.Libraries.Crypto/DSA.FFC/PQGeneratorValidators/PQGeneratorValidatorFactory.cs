using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class PQGeneratorValidatorFactory : IPQGeneratorValidatorFactory
    {
        public IPQGeneratorValidator GetGeneratorValidator(PrimeGenMode primeGenMode, ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            switch (primeGenMode)
            {
                case PrimeGenMode.Probable:
                    return new ProbablePQGeneratorValidator(sha, entropyType);

                case PrimeGenMode.Provable:
                    return new ProvablePQGeneratorValidator(sha, entropyType);

                default:
                    throw new ArgumentOutOfRangeException("Invalid PrimeGenMode provided");
            }
        }
    }
}
