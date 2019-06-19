using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.Crypto.HMAC
{
    public class FastHmacFactory : IFastHmacFactory
    {
        private readonly IShaFactory _iShaFactory;

        private readonly HashFunction[] _validFunctions =
        {
            new HashFunction(ModeValues.SHA1, DigestSizes.d160),
            new HashFunction(ModeValues.SHA2, DigestSizes.d256),
            new HashFunction(ModeValues.SHA2, DigestSizes.d384),
            new HashFunction(ModeValues.SHA2, DigestSizes.d512)
        };
        
        public FastHmacFactory(IShaFactory iShaFactory)
        {
            _iShaFactory = iShaFactory;
        }
        
        public IHmac GetHmacInstance(HashFunction hashFunction)
        {
            var sha = _iShaFactory.GetShaInstance(hashFunction);

            if (_validFunctions.Contains(hashFunction))
            {
                return new FastHmac(sha);
            }
            
            return new Hmac(sha);
        }
    }
}