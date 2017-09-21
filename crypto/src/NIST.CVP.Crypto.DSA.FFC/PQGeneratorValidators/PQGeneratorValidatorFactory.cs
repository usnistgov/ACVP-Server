using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class PQGeneratorValidatorFactory
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
