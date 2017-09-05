using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class CanonicalGeneratorGeneratorValidator : IGGeneratorValidator
    {
        public GGenerateResult Generate(BigInteger p, BigInteger q)
        {
            throw new NotImplementedException();
        }

        public GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g)
        {
            throw new NotImplementedException();
        }
    }
}
