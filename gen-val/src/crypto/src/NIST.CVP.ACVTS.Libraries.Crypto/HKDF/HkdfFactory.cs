using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.HKDF
{
    public class HkdfFactory : IHkdfFactory
    {
        private readonly IHmacFactory _hmacFactory;

        public HkdfFactory(IHmacFactory hmacFactory)
        {
            _hmacFactory = hmacFactory;
        }

        public IHkdf GetKdf(HashFunction hmacAlg)
        {
            var hmac = _hmacFactory.GetHmacInstance(hmacAlg);
            return new Hkdf(hmac);
        }
    }
}
