using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA2
{
    public interface ISHA_MCT
    {
        MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, bool isSample);
    }
}
