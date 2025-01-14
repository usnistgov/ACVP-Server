using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum SLHDSASignatureDisposition
{
    [EnumMember(Value = "valid signature and message - signature should verify successfully")]
    None,
    
    [EnumMember(Value = "modified message")]
    ModifyMessage,
    
    [EnumMember(Value = "modified signature - R")]
    ModifySignatureR,
    
    [EnumMember(Value = "modified signature - SIGFORS")]
    ModifySignatureSigFors,
    
    [EnumMember(Value = "modified signature - SIGHT")]
    ModifySignatureSigHt,
    
    [EnumMember(Value = "invalid signature - too large")]
    ModifySignatureTooLarge,
    
    [EnumMember(Value = "invalid signature - too small")]
    ModifySignatureTooSmall
}
