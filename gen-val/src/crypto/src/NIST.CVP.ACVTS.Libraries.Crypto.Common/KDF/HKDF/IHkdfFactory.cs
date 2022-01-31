using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF
{
    public interface IHkdfFactory
    {
        IHkdf GetKdf(HashFunction hmacAlg);
    }
}
