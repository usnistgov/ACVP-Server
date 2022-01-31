using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators
{
    public interface IGGeneratorValidatorFactory
    {
        IGGeneratorValidator GetGeneratorValidator(GeneratorGenMode genMode, ISha sha);
    }
}
