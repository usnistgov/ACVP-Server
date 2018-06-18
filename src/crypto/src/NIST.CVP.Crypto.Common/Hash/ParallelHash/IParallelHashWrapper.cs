using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHashWrapper
    {
        BitString HashMessage(BitString message, int digestSize, int capacity, int blockSize, bool xof, string customization = "");
    }
}
