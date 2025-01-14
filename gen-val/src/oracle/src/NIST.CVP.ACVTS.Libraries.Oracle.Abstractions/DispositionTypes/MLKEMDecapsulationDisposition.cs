using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum MLKEMDecapsulationDisposition
{
    [EnumMember(Value = "valid decapsulation")]
    None,
    
    [EnumMember(Value = "modified ciphertext")]
    ModifyCiphertext
}
