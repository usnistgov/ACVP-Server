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
                    throw new NotImplementedException();

                case GeneratorGenMode.Unverifiable:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException("Bad Generator Gen Mode");
            }
        }
    }
}
