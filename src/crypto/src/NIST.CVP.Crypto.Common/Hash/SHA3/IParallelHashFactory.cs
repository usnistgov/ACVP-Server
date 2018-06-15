namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface IParallelHashFactory
    {
        IParallelHashWrapper GetParallelHash(HashFunction hashFunction);
    }
}
