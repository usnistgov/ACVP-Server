using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes
{
    public interface ICounterModeBlockCipher
    {
        SymmetricCounterResult ExtractIvs(ICounterModeBlockCipherParameters parameters);
    }
}
