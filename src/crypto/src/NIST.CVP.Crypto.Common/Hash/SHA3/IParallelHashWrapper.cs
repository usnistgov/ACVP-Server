using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface IParallelHashWrapper
    {
        BitString HashMessage(BitString message, int digestSize, int capacity, int blockSize, bool xof, string customization = "");
    }
}
