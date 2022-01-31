using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PssMaskTypes
    {
        None,

        [EnumMember(Value = "mgf1")]
        MGF1,

        [EnumMember(Value = "shake-128")]
        SHAKE128,

        [EnumMember(Value = "shake-256")]
        SHAKE256
    }
}
