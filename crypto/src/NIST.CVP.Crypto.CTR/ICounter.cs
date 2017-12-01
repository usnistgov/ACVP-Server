using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    public interface ICounter
    {
        BitString GetNextIV();
    }
}
