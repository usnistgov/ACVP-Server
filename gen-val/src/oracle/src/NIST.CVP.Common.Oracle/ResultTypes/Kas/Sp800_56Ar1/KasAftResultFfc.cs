using System.Numerics;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1
{
    public class KasAftResultFfc : KasAftResultBase
    {
        public BigInteger StaticPrivateKeyServer { get; set; }

        public BigInteger StaticPublicKeyServer { get; set; }

        public BigInteger EphemeralPrivateKeyServer { get; set; }

        public BigInteger EphemeralPublicKeyServer { get; set; }


        #region Sample Only Properties
        public BigInteger StaticPrivateKeyIut { get; set; }

        public BigInteger StaticPublicKeyIut { get; set; }

        public BigInteger EphemeralPrivateKeyIut { get; set; }

        public BigInteger EphemeralPublicKeyIut { get; set; }
        #endregion Sample Only Properties
    }
}