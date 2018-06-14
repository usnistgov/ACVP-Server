namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ITupleHashFactory
    {
        ITupleHashWrapper GetTupleHash(HashFunction hashFunction);
    }
}
