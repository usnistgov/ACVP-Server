using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.MonteCarlo
{
    public interface IMonteCarloKeyMakerTdes
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults);
    }
}
