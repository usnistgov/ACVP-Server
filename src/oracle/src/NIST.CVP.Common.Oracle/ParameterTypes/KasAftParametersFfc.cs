using System.Numerics;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KasAftParametersFfc : KasAftParametersBase
    {
        public FfcScheme FfcScheme { get; set; }

        public FfcParameterSet FfcParameterSet { get; set; }


        public BitString P { get; set; }

        public BitString Q { get; set; }

        public BitString G { get; set; }
    }
}