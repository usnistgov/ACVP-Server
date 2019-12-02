using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.KDF.Components.PBKDF
{
    public interface IPbKdfFactory
    {
        IPbKdf GetKdf(HashFunction hashFunction);
    }
}