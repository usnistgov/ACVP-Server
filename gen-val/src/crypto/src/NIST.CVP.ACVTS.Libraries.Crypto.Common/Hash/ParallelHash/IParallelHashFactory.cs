namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ParallelHash
{
    public interface IParallelHashFactory
    {
        IParallelHashWrapper GetParallelHash(HashFunction hashFunction);
    }
}
