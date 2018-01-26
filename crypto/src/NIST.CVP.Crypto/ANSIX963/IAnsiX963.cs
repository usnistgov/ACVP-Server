using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.ANSIX963
{
    public interface IAnsiX963
    {
        KdfResult DeriveKey(BitString z, BitString sharedInfo, int keyLength);
    }
}
