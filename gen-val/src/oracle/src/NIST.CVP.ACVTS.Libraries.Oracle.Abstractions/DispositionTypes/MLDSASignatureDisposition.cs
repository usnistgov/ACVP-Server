using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum MLDSASignatureDisposition
{
    // TODO add HintCheck, CommitmentHash (covered by modify signature and modify message)
    
    [EnumMember(Value = "valid signature and message - signature should verify successfully")]
    None,
    
    [EnumMember(Value = "modified signature - commitment")]
    ModifySignature,
    
    [EnumMember(Value = "modified message")]
    ModifyMessage,
    
    [EnumMember(Value = "modified signature - hint")]
    ModifyHint,
    
    [EnumMember(Value = "modified signature - z")]
    ModifyZ,
    
    [EnumMember(Value = "invalid signature - z norm too large")]
    LargeZNorm
}
