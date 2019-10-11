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
        [EnumMember(Value = "IKEv1")]
        Ike_v1,
        [EnumMember(Value = "IKEv2")]
        Ike_v2
    }
}