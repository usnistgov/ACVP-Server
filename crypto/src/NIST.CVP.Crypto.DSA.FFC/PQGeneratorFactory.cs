using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class PQGeneratorFactory
    {
        public IPQGenerator GetGenerator(PrimeGenMode primeGenMode)
        {
            switch (primeGenMode)
            {
                case PrimeGenMode.ProbableLegacy:
                    throw new NotImplementedException();

                case PrimeGenMode.Probable:
                    throw new NotImplementedException();

                case PrimeGenMode.Provable:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException("Invalid PrimeGenMode provided");
            }
        }
    }
}
