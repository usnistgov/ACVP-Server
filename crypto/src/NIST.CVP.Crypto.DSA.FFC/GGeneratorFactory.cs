using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class GGeneratorFactory
    {
        public IGGenerator GetGenerator(GeneratorGenMode genMode)
        {
            switch (genMode)
            {
                case GeneratorGenMode.Canonical:
                    return new CanonicalGeneratorGenerator();

                case GeneratorGenMode.Unverifiable:
                    return new UnverifiableGeneratorGenerator();

                default:
                    throw new ArgumentOutOfRangeException("Bad Generator Gen Mode");
            }
        }
    }
}
