using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo
{
    public interface IFixedInfoStrategy
    {
        BitString GetFixedInfo(Dictionary<string, BitString> fixedInfoParts);
    }
}
