using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators
{
    public interface IPQGeneratorValidatorFactory
    {
        IPQGeneratorValidator GetGeneratorValidator(PrimeGenMode primeGenMode, ISha sha, EntropyProviderTypes entropyType);
    }
}
