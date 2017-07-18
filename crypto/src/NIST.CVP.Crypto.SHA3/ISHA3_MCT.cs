using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA3
{
    public interface ISHA3_MCT
    {
        MCTResult<AlgoArrayResponse> MCTHash(HashFunction function, BitString message, bool isSample, int min, int max);
    }
}
