using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.HMAC
{
    public class HmacFactory : IHmacFactory
    {
        private readonly IShaFactory _iShaFactory;

        public HmacFactory(IShaFactory iShaFactory)
        {
            _iShaFactory = iShaFactory;
        }

        public IHmac GetHmacInstance(HashFunction hashFunction)
        {
            var sha = _iShaFactory.GetShaInstance(hashFunction);

            return new NativeHmac(sha);
        }
    }
}
