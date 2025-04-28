using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class XecdhSscDeferredParameters
    {
        public Curve Curve { get; set; }

        public BitString PrivateKeyServer { get; set; }

        public BitString PublicKeyServer { get; set; }

        public BitString PublicKeyIut { get; set; }
    }
}
