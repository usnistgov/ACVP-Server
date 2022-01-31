using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo
{
    public interface IMonteCarloKeyMakerAes
    {
        BitString MixKeys(BitString currentKey, BitString lastOutput, BitString secondToLastOutput);
    }
}
