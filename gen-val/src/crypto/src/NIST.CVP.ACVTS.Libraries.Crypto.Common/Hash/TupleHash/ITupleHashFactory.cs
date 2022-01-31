namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHashFactory
    {
        ITupleHashWrapper GetTupleHash(HashFunction hashFunction);
    }
}
