using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHash_MCT
    {
        MCTResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain outputLength, MathDomain blockSize, bool hexCustomization, bool isSample);
    }
}
