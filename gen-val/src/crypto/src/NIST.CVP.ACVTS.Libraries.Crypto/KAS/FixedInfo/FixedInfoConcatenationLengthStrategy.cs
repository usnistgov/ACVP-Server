using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo
{
    /// <summary>
    /// This strategy specifies the length (via 32 bit counter) of the then following piece of fixed info,
    /// utilizing concatenation until all pieces are written. 
    /// </summary>
    public class FixedInfoConcatenationLengthStrategy : IFixedInfoStrategy
    {
        public BitString GetFixedInfo(Dictionary<string, BitString> fixedInfoParts)
        {
            BitString fixedInfo = new BitString(0);

            foreach (var pair in fixedInfoParts)
            {
                fixedInfo = fixedInfo
                    .ConcatenateBits(BitString.To32BitString(pair.Value.BitLength))
                    .ConcatenateBits(pair.Value);
            }

            return fixedInfo;
        }
    }
}
