using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CBC
{
    public interface IMonteCarloKeyMaker
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults);
    }
}