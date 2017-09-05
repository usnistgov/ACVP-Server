using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class ProbablePQGeneratorValidator : IPQGeneratorValidator
    {
        public ProbablePQGeneratorValidator(ISha sha)
        {

        }

        public PQGenerateResult Generate(int L, int N, int seedLen)
        {
            throw new NotImplementedException();
        }

        public PQValidateResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
