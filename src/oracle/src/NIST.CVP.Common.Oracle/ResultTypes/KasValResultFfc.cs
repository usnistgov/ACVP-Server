using System.Numerics;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class KasValResultFfc : KasValResultBase
    {
        public BigInteger StaticPrivateKeyServer { get; set; }

        public BigInteger StaticPublicKeyServerX { get; set; }

        public BigInteger StaticPublicKeyServerY { get; set; }

        public BigInteger EphemeralPrivateKeyServer { get; set; }

        public BigInteger EphemeralPublicKeyServerX { get; set; }

        public BigInteger EphemeralPublicKeyServerY { get; set; }


        public BigInteger StaticPrivateKeyIut { get; set; }

        public BigInteger StaticPublicKeyIutX { get; set; }

        public BigInteger StaticPublicKeyIutY { get; set; }

        public BigInteger EphemeralPrivateKeyIut { get; set; }

        public BigInteger EphemeralPublicKeyIutX { get; set; }

        public BigInteger EphemeralPublicKeyIutY { get; set; }
    }
}
