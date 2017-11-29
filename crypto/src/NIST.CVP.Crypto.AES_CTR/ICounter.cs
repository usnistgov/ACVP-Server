using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.AES_CTR
{
    public interface ICounter
    {
        BitString GetNextIV();
    }
}
