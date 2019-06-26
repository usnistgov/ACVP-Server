using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class KasAftResultFfc : KasAftResultBase
    {
        public BitString StaticPrivateKeyServer { get; set; }

        public BitString StaticPublicKeyServer { get; set; }

        public BitString EphemeralPrivateKeyServer { get; set; }

        public BitString EphemeralPublicKeyServer { get; set; }


        #region Sample Only Properties
        public BitString StaticPrivateKeyIut { get; set; }

        public BitString StaticPublicKeyIut { get; set; }

        public BitString EphemeralPrivateKeyIut { get; set; }

        public BitString EphemeralPublicKeyIut { get; set; }
        #endregion Sample Only Properties
    }
}