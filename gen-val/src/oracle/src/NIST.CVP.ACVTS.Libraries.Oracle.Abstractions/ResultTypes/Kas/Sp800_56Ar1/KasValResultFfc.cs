using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1
{
    public class KasValResultFfc : KasValResultBase
    {
        public BigInteger StaticPrivateKeyServer { get; set; }

        public BigInteger StaticPublicKeyServer { get; set; }


        public BigInteger EphemeralPrivateKeyServer { get; set; }

        public BigInteger EphemeralPublicKeyServer { get; set; }



        public BigInteger StaticPrivateKeyIut { get; set; }

        public BigInteger StaticPublicKeyIut { get; set; }


        public BigInteger EphemeralPrivateKeyIut { get; set; }

        public BigInteger EphemeralPublicKeyIut { get; set; }
    }
}
