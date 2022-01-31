using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasAftParametersFfc : KasAftParametersBase
    {
        public FfcScheme FfcScheme { get; set; }

        public FfcParameterSet FfcParameterSet { get; set; }


        public BigInteger P { get; set; }

        public BigInteger Q { get; set; }

        public BigInteger G { get; set; }
    }
}
