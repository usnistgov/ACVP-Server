using System.ComponentModel;

namespace NIST.CVP.Crypto.DRBG.Enums
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