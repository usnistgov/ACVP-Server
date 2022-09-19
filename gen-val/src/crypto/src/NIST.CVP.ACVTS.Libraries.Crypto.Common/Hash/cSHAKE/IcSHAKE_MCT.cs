using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE
{
    public interface IcSHAKE_MCT
    {
        MctResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain domain, bool customizationHex, bool isSample);
    }
}
