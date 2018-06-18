using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.Crypto.KMAC
{
    public class KmacFactory : IKmacFactory
    {
        private readonly ICSHAKEFactory _iCSHAKEFactory;

        public KmacFactory(ICSHAKEFactory iCSHAKEFactory)
        {
            _iCSHAKEFactory = iCSHAKEFactory;
        }

        public IKmac GetKmacInstance(HashFunction hashFunction, bool xof)
        {
            var cshake = _iCSHAKEFactory.GetCSHAKE(hashFunction);

            return new Kmac(cshake, hashFunction.Capacity, xof);
        }
    }
}
