using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public interface IMonteCarloKeyMaker
    {
        BitString MixKeys(TDESKeys keys, List<BitString> lastThreeOpResults);
    }
}
