using System.Numerics;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KasValParametersFfc : KasValParametersBase
    {
        public FfcScheme FfcScheme { get; set; }

        public FfcParameterSet FfcParameterSet { get; set; }


        public BigInteger P { get; set; }

        public BigInteger Q { get; set; }

        public BigInteger G { get; set; }
    }
}
