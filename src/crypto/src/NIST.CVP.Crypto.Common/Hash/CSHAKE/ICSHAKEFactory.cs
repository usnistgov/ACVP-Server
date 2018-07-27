using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKEFactory
    {
        ICSHAKEWrapper GetCSHAKE(HashFunction hashFunction);
    }
}
