using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators
{
    public interface IGGeneratorValidator
    {
        GGenerateResult Generate(BigInteger p, BigInteger q, DomainSeed seed = null, BitString index = null);
        GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g, DomainSeed seed = null, BitString index = null);
    }
}
