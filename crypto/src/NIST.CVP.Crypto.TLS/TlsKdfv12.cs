using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;

namespace NIST.CVP.Crypto.TLS
{
    public class TlsKdfv12 : ITlsKdf
    {
        private readonly IHmac _hmac;

        public TlsKdfv12(IHmac hmac)
        {
            _hmac = hmac;
        }

        public TlsKdfResult DeriveKey()
        {
            throw new NotImplementedException();
        }
    }
}
