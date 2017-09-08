using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class CanonicalGeneratorGeneratorValidator : IGGeneratorValidator
    {
        public GGenerateResult Generate(BigInteger p, BigInteger q, DomainSeed seed = null, int index = 0)
        {
            throw new NotImplementedException();
        }

        public GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g, DomainSeed seed = null, int index = 0)
        {
            throw new NotImplementedException();
        }
    }
}
