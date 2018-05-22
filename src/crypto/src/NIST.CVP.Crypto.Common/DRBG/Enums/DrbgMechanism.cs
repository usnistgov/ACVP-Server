using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.DRBG.Enums
{
    public enum DrbgMechanism
    {
        [EnumMember(Value = "ctrDRBG")]
        Counter,

        [EnumMember(Value = "hashDRBG")]
        Hash,

        [EnumMember(Value = "hmacDRBG")]
        HMAC
    }
}