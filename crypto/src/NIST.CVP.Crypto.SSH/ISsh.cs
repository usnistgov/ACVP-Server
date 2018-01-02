using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SSH
{
    public interface ISsh
    {
        KdfResult DeriveKey(BitString k, BitString h, BitString sessionId);
    }
}
