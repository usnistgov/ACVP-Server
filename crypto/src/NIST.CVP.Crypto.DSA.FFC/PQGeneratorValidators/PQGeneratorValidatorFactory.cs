using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class PQGeneratorValidatorFactory
    {
        public IPQGeneratorValidator GetGeneratorValidator(PrimeGenMode primeGenMode, ISha sha)
        {
            switch (primeGenMode)
            {
                case PrimeGenMode.ProbableLegacy:
                    return new LegacyProbablePQGeneratorValidator(sha);

                case PrimeGenMode.Probable:
                    return new ProbablePQGeneratorValidator(sha);

                case PrimeGenMode.Provable:
                    return new ProvablePQGeneratorValidator(sha);

                default:
                    throw new ArgumentOutOfRangeException("Invalid PrimeGenMode provided");
            }
        }
    }
}
