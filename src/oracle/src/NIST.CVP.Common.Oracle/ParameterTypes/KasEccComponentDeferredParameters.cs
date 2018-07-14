using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KasEccComponentDeferredParameters
    {
        public Curve Curve { get; set; }


        public BigInteger PrivateKeyServer { get; set; }

        public BigInteger PublicKeyServerX { get; set; }

        public BigInteger PublicKeyServerY { get; set; }


        public BigInteger PublicKeyIutX { get; set; }
                                   
        public BigInteger PublicKeyIutY { get; set; }

        public BitString Z { get; set; }
    }
}