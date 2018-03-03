using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.Crypto.IKEv2
{
    public class IkeV2Factory : IIkeV2Factory
    {
        private readonly IHmacFactory _hmacFactory;

        public IkeV2Factory(IHmacFactory hmacFactory)
        {
            _hmacFactory = hmacFactory;
        }

        public IIkeV2 GetInstance(HashFunction hashFunction)
        {
            var hmac = _hmacFactory.GetHmacInstance(hashFunction);

            return new IkeV2(hmac);
        }
    }
}