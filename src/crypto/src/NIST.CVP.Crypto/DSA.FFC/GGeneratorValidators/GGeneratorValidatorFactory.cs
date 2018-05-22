using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class GGeneratorValidatorFactory : IGGeneratorValidatorFactory
    {
        public IGGeneratorValidator GetGeneratorValidator(GeneratorGenMode genMode, ISha sha)
        {
            switch (genMode)
            {
                case GeneratorGenMode.Canonical:
                    return new CanonicalGeneratorGeneratorValidator(sha);

                case GeneratorGenMode.Unverifiable:
                    return new UnverifiableGeneratorGeneratorValidator();

                default:
                    throw new ArgumentOutOfRangeException("Bad Generator Gen Mode");
            }
        }
    }
}
