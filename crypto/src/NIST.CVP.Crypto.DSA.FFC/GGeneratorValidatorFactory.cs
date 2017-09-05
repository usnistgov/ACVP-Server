using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class GGeneratorValidatorFactory
    {
        public IGGeneratorValidator GetGenerator(GeneratorGenMode genMode)
        {
            switch (genMode)
            {
                case GeneratorGenMode.Canonical:
                    return new CanonicalGeneratorGeneratorValidator();

                case GeneratorGenMode.Unverifiable:
                    return new UnverifiableGeneratorGeneratorValidator();

                default:
                    throw new ArgumentOutOfRangeException("Bad Generator Gen Mode");
            }
        }
    }
}
