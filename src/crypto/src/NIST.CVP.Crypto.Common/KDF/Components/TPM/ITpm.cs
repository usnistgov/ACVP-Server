using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.TPM
{
    public interface ITpm
    {
        KdfResult DeriveKey(BitString auth, BitString nonceEven, BitString nonceOdd);
    }
}
