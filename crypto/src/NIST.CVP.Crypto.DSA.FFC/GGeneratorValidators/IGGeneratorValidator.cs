using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public interface IGGeneratorValidator
    {
        GGenerateResult Generate(BigInteger p, BigInteger q, DomainSeed seed = null, BitString index = null);
        GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g, DomainSeed seed = null, BitString index = null);
    }
}
