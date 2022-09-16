using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum SignatureModifications
    {
        [EnumMember(Value = "No modification, i.e., \"testPassed\": true expected")]
        None,

        [EnumMember(Value = "Key modified")]
        E,

        [EnumMember(Value = "Message modified")]
        Message,

        [EnumMember(Value = "Signature modified")]
        Signature,

        [EnumMember(Value = "IR moved from expected location")]
        MoveIr,

        [EnumMember(Value = "IR trailer modified from expected value")]
        ModifyTrailer
    }
}
