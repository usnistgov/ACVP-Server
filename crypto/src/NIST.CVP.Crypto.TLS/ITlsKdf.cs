using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.TLS
{
    public interface ITlsKdf
    {
        TlsKdfResult DeriveKey();
    }
}
