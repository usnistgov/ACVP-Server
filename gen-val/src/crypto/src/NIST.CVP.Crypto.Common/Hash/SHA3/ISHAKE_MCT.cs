using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ISHAKE_MCT
    {
        MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, MathDomain domain, bool isSample);
    }
}
