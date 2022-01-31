using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum Kda
    {
        None,
        [EnumMember(Value = "oneStep")]
        OneStep,
        [EnumMember(Value = "oneStepNoCounter")]
        OneStepNoCounter,
        [EnumMember(Value = "twoStep")]
        TwoStep,
        [EnumMember(Value = "IKEv1")]
        Ike_v1,
        [EnumMember(Value = "IKEv2")]
        Ike_v2,
        [EnumMember(Value = "tls_v1.0/v1.1")]
        Tls_v10_v11,
        [EnumMember(Value = "tls_v1.2")]
        Tls_v12,
        [EnumMember(Value = "hkdf")]
        Hkdf
    }
}
