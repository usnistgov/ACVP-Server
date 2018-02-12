using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum SignatureSchemes
    {
        [Description("ansx9.31")]
        Ansx931,

        [Description("pkcs1v15")]
        Pkcs1v15,

        [Description("pss")]
        Pss
    }
}
