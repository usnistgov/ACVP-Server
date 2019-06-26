using System.Numerics;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KasAftDeferredParametersFfc : KasAftDeferredParametersBase
    {
        public FfcScheme FfcScheme { get; set; }

        public FfcParameterSet FfcParameterSet { get; set; }


        public BitString P { get; set; }

        public BitString Q { get; set; }

        public BitString G { get; set; }


        public BitString StaticPrivateKeyServer { get; set; }

        public BitString StaticPublicKeyServer { get; set; }

        public BitString EphemeralPrivateKeyServer { get; set; }

        public BitString EphemeralPublicKeyServer { get; set; }
        

        public BitString StaticPublicKeyIut { get; set; }

        public BitString EphemeralPublicKeyIut { get; set; }
    }
}