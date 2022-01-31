using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasEccComponentDeferredParameters
    {
        public Curve Curve { get; set; }


        public BigInteger PrivateKeyServer { get; set; }

        public BigInteger PublicKeyServerX { get; set; }

        public BigInteger PublicKeyServerY { get; set; }


        public BigInteger PublicKeyIutX { get; set; }

        public BigInteger PublicKeyIutY { get; set; }
    }
}
