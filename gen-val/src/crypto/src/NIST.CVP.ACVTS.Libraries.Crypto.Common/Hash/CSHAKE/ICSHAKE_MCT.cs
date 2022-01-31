using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKE_MCT
    {
        MctResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain domain, bool customizationHex, bool isSample);
    }
}
