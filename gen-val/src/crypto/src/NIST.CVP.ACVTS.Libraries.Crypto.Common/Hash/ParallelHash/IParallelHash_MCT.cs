using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHash_MCT
    {
        MctResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain outputLength, MathDomain blockSize, bool hexCustomization, bool isSample);
    }
}
