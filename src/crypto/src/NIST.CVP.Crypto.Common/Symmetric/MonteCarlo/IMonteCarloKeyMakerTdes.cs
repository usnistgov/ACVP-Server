using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.MonteCarlo
{
    public interface IMonteCarloKeyMakerTdes
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults);
    }
}