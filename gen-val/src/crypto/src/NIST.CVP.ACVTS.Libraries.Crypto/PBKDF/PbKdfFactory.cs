using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.PBKDF
{
    public class PbKdfFactory : IPbKdfFactory
    {
        private readonly IHmacFactory _hmacFactory;

        public PbKdfFactory(IHmacFactory hmacFactory)
        {
            _hmacFactory = hmacFactory;
        }

        public IPbKdf GetKdf(HashFunction hashFunction)
        {
            var hmac = _hmacFactory.GetHmacInstance(hashFunction);
            return new PbKdf(hmac);
        }
    }
}
