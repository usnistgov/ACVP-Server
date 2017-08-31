using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Crypto.KAS
{
    public static class FfcParameterSetDetails
    {
        public static readonly
            Dictionary<FfcParameterSet, (int lLength, int nLength, int minHashLength, int minMacKeyLength, int
                minMacLength)> Details =
                new Dictionary<FfcParameterSet, (int lLength, int nLength, int minHashLength, int minMacKeyLength, int
                    minMacLength)>()
                {
                    {FfcParameterSet.FB, ( 2048, 224, 224, 112, 112 )},
                    {FfcParameterSet.FC, ( 2048, 256, 256, 128, 128 )}
                };
    }
}
