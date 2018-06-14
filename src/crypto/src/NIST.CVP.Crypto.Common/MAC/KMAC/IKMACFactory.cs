using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.MAC.KMAC
{
    public interface IKmacFactory
    {
        IKmac GetKmacInstance(HashFunction hashFunction, bool xof);
    }
}
