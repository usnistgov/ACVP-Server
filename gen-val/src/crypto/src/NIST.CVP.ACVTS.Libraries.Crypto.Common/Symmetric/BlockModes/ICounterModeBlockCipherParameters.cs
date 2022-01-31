using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes
{
    public interface ICounterModeBlockCipherParameters : IModeBlockCipherParameters
    {
        BitString Result { get; set; }
    }
}
