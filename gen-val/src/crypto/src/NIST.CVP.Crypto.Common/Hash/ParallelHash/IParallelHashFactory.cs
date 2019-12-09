using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHashFactory
    {
        IParallelHashWrapper GetParallelHash(HashFunction hashFunction);
    }
}
