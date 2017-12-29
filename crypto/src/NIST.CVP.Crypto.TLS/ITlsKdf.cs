using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TLS
{
    public interface ITlsKdf
    {
        TlsKdfResult DeriveKey(BitString preMasterSecret, BitString clientHelloRandom, BitString serverHelloRandom, BitString clientRandom, BitString serverRandom, int keyBlockLength);
    }
}
