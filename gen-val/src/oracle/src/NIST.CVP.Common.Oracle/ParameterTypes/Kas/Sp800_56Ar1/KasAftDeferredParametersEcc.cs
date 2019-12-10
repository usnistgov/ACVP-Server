using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasAftDeferredParametersEcc : KasAftDeferredParametersBase
    {
        public Curve Curve { get; set; }

        public EccScheme EccScheme { get; set; }

        public EccParameterSet EccParameterSet { get; set; }

        public BigInteger StaticPrivateKeyServer { get; set; }

        public BigInteger StaticPublicKeyServerX { get; set; }

        public BigInteger StaticPublicKeyServerY { get; set; }

        public BigInteger EphemeralPrivateKeyServer { get; set; }

        public BigInteger EphemeralPublicKeyServerX { get; set; }

        public BigInteger EphemeralPublicKeyServerY { get; set; }


        public BigInteger StaticPublicKeyIutX { get; set; }

        public BigInteger StaticPublicKeyIutY { get; set; }

        public BigInteger EphemeralPublicKeyIutX { get; set; }

        public BigInteger EphemeralPublicKeyIutY { get; set; }
    }
}