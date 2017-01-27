using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public interface IMonteCarloKeyMaker
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeCipherTexts);
    }
}