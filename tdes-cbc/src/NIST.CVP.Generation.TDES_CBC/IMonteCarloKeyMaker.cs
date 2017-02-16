using System.Collections.Generic;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CBC
{
    public interface IMonteCarloKeyMaker
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults);
    }
}