using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum KasKdf
    {
        None,
        [EnumMember(Value = "oneStep")]
        OneStep,
        [EnumMember(Value = "twoStep")]
        TwoStep,
    }
}