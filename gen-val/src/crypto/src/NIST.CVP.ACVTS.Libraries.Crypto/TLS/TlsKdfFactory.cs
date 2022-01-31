using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.MD5;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TLS
{
    public class TlsKdfFactory : ITlsKdfFactory
    {
        private readonly IHmacFactory _hmacFactory;

        public TlsKdfFactory(IHmacFactory hmacFactory)
        {
            _hmacFactory = hmacFactory;
        }

        public ITlsKdf GetTlsKdfInstance(TlsModes tlsMode, HashFunction hash)
        {
            switch (tlsMode)
            {
                case TlsModes.v10v11:
                    if (hash.Mode != ModeValues.SHA1 || hash.DigestSize != DigestSizes.d160)
                    {
                        throw new ArgumentException("Invalid hash function");
                    }
                    return new TlsKdfv10v11(_hmacFactory.GetHmacInstance(hash), new HmacMd5(new Md5()));

                case TlsModes.v12:
                case TlsModes.v12_extendedMasterSecret:
                    if (hash.Mode != ModeValues.SHA2 || hash.DigestSize == DigestSizes.d224 || hash.DigestSize == DigestSizes.d160 || hash.DigestSize == DigestSizes.d512t224 || hash.DigestSize == DigestSizes.d512t256)
                    {
                        throw new ArgumentException("Invalid hash function");
                    }

                    return tlsMode == TlsModes.v12 ?
                        new TlsKdfv12(_hmacFactory.GetHmacInstance(hash)) :
                        new TlsKdfv12_ExtendedMasterSecret(_hmacFactory.GetHmacInstance(hash));

                default:
                    throw new ArgumentException("Invalid tls mode");
            }
        }
    }
}
