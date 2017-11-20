using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Crypto.KAS.Helpers
{
    public static class ParameterSetDetails
    {
        public static readonly
            Dictionary<FfcParameterSet, (int pLength, int qLength, int minHashLength, int minMacKeyLength, int
                minMacLength)> FfcDetails =
                new Dictionary<FfcParameterSet, (int pLength, int qLength, int minHashLength, int minMacKeyLength, int
                    minMacLength)>()
                {
                    {FfcParameterSet.Fb, ( 2048, 224, 224, 112, 112 )},
                    {FfcParameterSet.Fc, ( 2048, 256, 256, 128, 128 )}
                };

        public static (int pLength, int qLength, int minHashLength, int minMacKeyLength, int minMacLength) GetDetailsForFfcParameterSet(FfcParameterSet parameterSet)
        {
            return FfcDetails.First(w => w.Key == parameterSet).Value;
        }
    }
}
