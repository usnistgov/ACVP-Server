using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum SignatureModifications
    {
        [EnumMember(Value = "No modification")]
        None,

		[EnumMember(Value = "Key modified")]        E,

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
