using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.TLS.Enums;

namespace NIST.CVP.Crypto.TLS
{
    public class TlsKdfFactory : ITlsKdfFactory
    {
        public ITlsKdf GetTlsKdfInstance(TlsModes tlsMode, HashFunction hash)
        {
            var hmacFactory = new HmacFactory(new ShaFactory());
            switch (tlsMode)
            {
                case TlsModes.v10v11:
                    if (hash.Mode != ModeValues.SHA1 && hash.DigestSize != DigestSizes.d160)
                    {
                        throw new ArgumentException("Invalid hash function");
                    }
                    return new TlsKdfv10v11(hmacFactory.GetHmacInstance(hash));

                case TlsModes.v12:
                    if (hash.Mode != ModeValues.SHA2 || hash.DigestSize == DigestSizes.d224)
                    {
                        throw new ArgumentException("Invalid hash function");
                    }
                    return new TlsKdfv12(hmacFactory.GetHmacInstance(hash));

                default:
                    throw new ArgumentException("Invalid tls mode");
            }
        }
    }
}
