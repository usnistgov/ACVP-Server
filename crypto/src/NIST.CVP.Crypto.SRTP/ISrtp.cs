using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SRTP
{
    public interface ISrtp
    {
        KdfResult DeriveKey(int keyLength, BitString keyMaster, BitString saltMaster, BitString kdr, BitString index, BitString srtcpIndex);
    }
}
