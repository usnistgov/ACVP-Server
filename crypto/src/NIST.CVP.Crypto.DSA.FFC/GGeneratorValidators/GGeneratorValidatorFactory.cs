using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class GGeneratorValidatorFactory
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
