using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash
{
    public interface IShaMct
    {
        MctResult<AlgoArrayResponse> MctHash(BitString message, bool isSample = false, MathDomain domain = null, int digestSize = 0, int smallestSupportedMessageLengthGreaterThanZero = 0);
    }
}
