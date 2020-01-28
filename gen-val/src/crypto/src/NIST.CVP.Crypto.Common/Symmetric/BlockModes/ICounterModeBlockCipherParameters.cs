using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes
{
    public interface ICounterModeBlockCipherParameters : IModeBlockCipherParameters
    {
        BitString Result { get; set; }
    }
}
