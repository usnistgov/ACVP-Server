using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators
{
    public interface IPQGeneratorValidatorFactory
    {
        IPQGeneratorValidator GetGeneratorValidator(PrimeGenMode primeGenMode, ISha sha, EntropyProviderTypes entropyType);
    }
}
