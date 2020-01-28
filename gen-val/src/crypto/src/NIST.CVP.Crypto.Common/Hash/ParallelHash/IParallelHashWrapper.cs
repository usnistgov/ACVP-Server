using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHashWrapper
    {
        BitString HashMessage(BitString message, int digestLength, int capacity, int blockSize, bool xof, string customization = "");
        BitString HashMessage(BitString message, int digestLength, int capacity, int blockSize, bool xof, BitString customizationHex);
    }
}
