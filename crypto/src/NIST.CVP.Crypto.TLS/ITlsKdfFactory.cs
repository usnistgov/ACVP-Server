using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.TLS.Enums;

namespace NIST.CVP.Crypto.TLS
{
    public interface ITlsKdfFactory
    {
        ITlsKdf GetTlsKdfInstance(TlsModes tlsMode, HashFunction hash);
    }
}
