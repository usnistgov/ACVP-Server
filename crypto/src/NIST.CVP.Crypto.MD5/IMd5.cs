using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.MD5
{
    public interface IMd5
    {
        HashResult Hash(BitString message);
    }
}
