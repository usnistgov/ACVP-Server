using System.Collections.Generic;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.FixedInfo
{
    public interface IFixedInfoStrategy
    {
        BitString GetFixedInfo(Dictionary<string, BitString> fixedInfoParts);
    }
}