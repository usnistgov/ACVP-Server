using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class UnverifiableGeneratorGeneratorValidator : IGGeneratorValidator
    {
        public GGenerateResult Generate(BigInteger p, BigInteger q)
        {
            throw new NotImplementedException();
        }

        public GValidateResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
