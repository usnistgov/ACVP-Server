using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public interface IMonteCarloKeyMaker
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults);
    }
}
