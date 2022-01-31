using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.PBKDF
{
    public interface IPbKdfFactory
    {
        IPbKdf GetKdf(HashFunction hashFunction);
    }
}
