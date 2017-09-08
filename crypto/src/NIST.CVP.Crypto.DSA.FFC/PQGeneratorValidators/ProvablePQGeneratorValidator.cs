using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class ProvablePQGeneratorValidator : IPQGeneratorValidator
    {
        public ProvablePQGeneratorValidator(ISha sha)
        {

        }

        public PQGenerateResult Generate(int L, int N, int seedLen)
        {
            throw new NotImplementedException();
        }

        public PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count)
        {
            throw new NotImplementedException();
        }
    }
}
