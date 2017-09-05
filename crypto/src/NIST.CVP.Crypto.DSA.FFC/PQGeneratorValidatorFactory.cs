using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class PQGeneratorValidatorFactory
    {
        public IPQGeneratorValidator GetGenerator(PrimeGenMode primeGenMode)
        {
            switch (primeGenMode)
            {
                case PrimeGenMode.ProbableLegacy:
                    return new LegacyProbablePQGenerator();

                case PrimeGenMode.Probable:
                    return new ProbablePQGenerator();

                case PrimeGenMode.Provable:
                    return new ProvablePQGenerator();

                default:
                    throw new ArgumentOutOfRangeException("Invalid PrimeGenMode provided");
            }
        }
    }
}
