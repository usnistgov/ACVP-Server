using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHash
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, string customization);
        HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, BitString customizationHex);
    }
}
