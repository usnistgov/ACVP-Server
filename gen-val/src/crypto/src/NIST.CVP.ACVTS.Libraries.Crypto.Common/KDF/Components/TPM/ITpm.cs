using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TPM
{
    public interface ITpm
    {
        KdfResult DeriveKey(BitString auth, BitString nonceEven, BitString nonceOdd);
    }
}
