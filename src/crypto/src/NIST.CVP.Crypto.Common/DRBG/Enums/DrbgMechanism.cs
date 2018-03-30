using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.DRBG.Enums
{
    public enum DrbgMechanism
    {
        [Description("ctrDRBG")]
        Counter,

        [Description("hashDRBG")]
        Hash,

        [Description("hmacDRBG")]
        HMAC
    }
}