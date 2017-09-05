using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public interface IPQGeneratorValidator
    {
        PQGenerateResult Generate(int L, int N, int seedLen);
        PQValidateResult Validate();
    }
}
