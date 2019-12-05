using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKE_MCT
    {
        MCTResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain domain, bool customizationHex, bool isSample);
    }
}
