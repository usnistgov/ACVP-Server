using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum SLHDSASignatureDisposition
{
    [EnumMember(Value = "valid signature and message - signature should verify successfully")]
    None,
    
    [EnumMember(Value = "message altered")]
    ModifyMessage,
    
    [EnumMember(Value = "modified signature - R modified")]
    ModifySignatureR,
    
    [EnumMember(Value = "modified signature - SIGFORS modified")]
    ModifySignatureSigFors,
    
    [EnumMember(Value = "modified signature - SIGHT modified")]
    ModifySignatureSigHt,
    
    [EnumMember(Value = "invalid signature - signature is too large")]
    ModifySignatureTooLarge,
    
    [EnumMember(Value = "invalid signature - signature is too small")]
    ModifySignatureTooSmall
}
