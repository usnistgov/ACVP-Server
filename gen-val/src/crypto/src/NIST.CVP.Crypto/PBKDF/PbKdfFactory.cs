using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.Crypto.PBKDF
{
    public class PbKdfFactory : IPbKdfFactory
    {
        private readonly IShaFactory _shaFactory;
        private readonly IFastHmacFactory _hmacFactory;

        public PbKdfFactory(IShaFactory shaFactory, IFastHmacFactory hmacFactory)
        {
            _shaFactory = shaFactory;
            _hmacFactory = hmacFactory;
        }

        public IPbKdf GetKdf(HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);
            return new PbKdf(sha, _hmacFactory);
        }
    }
}