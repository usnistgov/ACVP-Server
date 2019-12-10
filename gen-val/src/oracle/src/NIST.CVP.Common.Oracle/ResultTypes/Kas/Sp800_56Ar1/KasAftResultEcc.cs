using System.Numerics;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1
{
    public class KasAftResultEcc : KasAftResultBase
    {
        public BigInteger StaticPrivateKeyServer { get; set; }

        public BigInteger StaticPublicKeyServerX { get; set; }

        public BigInteger StaticPublicKeyServerY { get; set; }

        public BigInteger EphemeralPrivateKeyServer { get; set; }

        public BigInteger EphemeralPublicKeyServerX { get; set; }

        public BigInteger EphemeralPublicKeyServerY { get; set; }


        #region Sample Only Properties
        public BigInteger StaticPrivateKeyIut { get; set; }

        public BigInteger StaticPublicKeyIutX { get; set; }

        public BigInteger StaticPublicKeyIutY { get; set; }

        public BigInteger EphemeralPrivateKeyIut { get; set; }

        public BigInteger EphemeralPublicKeyIutX { get; set; }

        public BigInteger EphemeralPublicKeyIutY { get; set; }
        #endregion Sample Only Properties
    }
}