using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface IParallelHash
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, int blockSize, string customization = "");
    }
}
