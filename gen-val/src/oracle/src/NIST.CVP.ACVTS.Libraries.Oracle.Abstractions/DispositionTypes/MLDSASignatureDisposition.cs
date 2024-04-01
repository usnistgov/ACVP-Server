using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum MLDSASignatureDisposition
{
    // TODO add HintCheck, LargeZNorm, CommitmentHash (covered by modify signature and modify message)
    
    [EnumMember(Value = "no modification")]
    None,
    
    [EnumMember(Value = "modify signature")]
    ModifySignature,
    
    [EnumMember(Value = "modify message")]
    ModifyMessage,
    
    [EnumMember(Value = "too many hints")]
    ModifyHint,
    
    [EnumMember(Value = "z too large")]
    ModifyZ
}
