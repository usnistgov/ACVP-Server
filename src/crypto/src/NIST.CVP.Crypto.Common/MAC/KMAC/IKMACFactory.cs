using NIST.CVP.Crypto.Common.Hash.CSHAKE;

namespace NIST.CVP.Crypto.Common.MAC.KMAC
{
    public interface IKmacFactory
    {
        IKmac GetKmacInstance(HashFunction hashFunction, bool xof);
    }
}
