using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.Hash.TupleHash
{
    public interface ITupleHashFactory
    {
        ITupleHashWrapper GetTupleHash(HashFunction hashFunction);
    }
}
