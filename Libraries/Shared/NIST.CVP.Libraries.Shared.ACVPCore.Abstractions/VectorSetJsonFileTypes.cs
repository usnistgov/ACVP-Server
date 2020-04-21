using System.Runtime.Serialization;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions
{
    public enum VectorSetJsonFileTypes
    {
        [EnumMember(Value = "capabilities")]
        Capabilities = 1,
        [EnumMember(Value = "prompt")]
        Prompt = 2,
        [EnumMember(Value = "internalProjection")]
        InternalProjection = 3,
        [EnumMember(Value = "expectedAnswers")]
        ExpectedAnswers = 4,
        [EnumMember(Value = "submittedAnswers")]
        SubmittedAnswers = 5,
        [EnumMember(Value = "validation")]
        Validation = 6,
        [EnumMember(Value = "error")]
        Error = 7
    }
}