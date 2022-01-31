using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHashWrapper
    {
        BitString HashMessage(BitString message, int digestLength, int capacity, int blockSize, bool xof, string customization = "");
        BitString HashMessage(BitString message, int digestLength, int capacity, int blockSize, bool xof, BitString customizationHex);
    }
}
