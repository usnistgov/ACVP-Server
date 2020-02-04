using System.Runtime.Serialization;

namespace ACVPCore
{
    public enum VectorSetJsonFileTypes
    {
        [EnumMember(Value = "capabilities")]
        Capabilities,
        [EnumMember(Value = "prompt")]
        Prompt,
        [EnumMember(Value = "internalProjection")]
        InternalProjection,
        [EnumMember(Value = "expectedAnswers")]
        ExpectedAnswers,
        [EnumMember(Value = "submittedAnswers")]
        SubmittedAnswers,
        [EnumMember(Value = "validation")]
        Validation,
        [EnumMember(Value = "error")]
        Error
    }
}