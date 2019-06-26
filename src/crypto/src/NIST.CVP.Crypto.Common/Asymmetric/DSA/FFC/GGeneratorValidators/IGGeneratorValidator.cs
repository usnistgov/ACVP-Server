using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators
{
    public interface IGGeneratorValidator
    {
        GGenerateResult Generate(BitString p, BitString q, DomainSeed seed = null, BitString index = null);
        GValidateResult Validate(BitString p, BitString q, BitString g, DomainSeed seed = null, BitString index = null);
    }
}
