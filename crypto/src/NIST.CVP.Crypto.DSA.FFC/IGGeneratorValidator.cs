using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public interface IGGeneratorValidator
    {
        GGenerateResult Generate(BigInteger p, BigInteger q);
        GValidateResult Validate();
    }
}
