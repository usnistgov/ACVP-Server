using System.ComponentModel;

namespace NIST.CVP.Crypto.DRBG.Enums
{
    public enum DrbgMode
    {
        [Description("AES-128")]
        AES128,

        [Description("AES-192")]
        AES192,

        [Description("AES-256")]
        AES256,

        [Description("TDES")]
        TDES
    }
}