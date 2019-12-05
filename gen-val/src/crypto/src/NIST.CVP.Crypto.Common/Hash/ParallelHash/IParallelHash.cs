using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHash
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, string customization);
        HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, BitString customizationHex);
    }
}
