using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum RsaSpDisposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "Failure - Message is equal to N")]
        MsgEqualN,
        
        [EnumMember(Value = "Failure - Message is > N but < modulo that is all 1's")]
        MsgGreaterNLessModulo,
    }
}