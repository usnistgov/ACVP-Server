using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.HMAC
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

            return new Hmac(sha);
        }
    }
}
