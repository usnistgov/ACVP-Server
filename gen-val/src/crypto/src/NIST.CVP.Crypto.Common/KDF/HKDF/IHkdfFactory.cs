using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.KDF.HKDF
{
    public interface IHkdfFactory
    {
        IHkdf GetKdf(HashFunction hmacAlg);
    }
}