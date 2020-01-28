using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.MonteCarlo
{
    public interface IMonteCarloKeyMakerAes
    {
        BitString MixKeys(BitString currentKey, BitString lastOutput, BitString secondToLastOutput);
    }
}