using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo
{
    /// <summary>
    /// This strategy concatenates all pieces making up the fixed info 
    /// </summary>
    public class FixedInfoConcatenationStrategy : IFixedInfoStrategy
    {
        public BitString GetFixedInfo(Dictionary<string, BitString> fixedInfoParts)
        {
            BitString fixedInfo = new BitString(0);

            foreach (var pair in fixedInfoParts)
            {
                fixedInfo = fixedInfo.ConcatenateBits(pair.Value);
            }

            return fixedInfo;
        }
    }
}
