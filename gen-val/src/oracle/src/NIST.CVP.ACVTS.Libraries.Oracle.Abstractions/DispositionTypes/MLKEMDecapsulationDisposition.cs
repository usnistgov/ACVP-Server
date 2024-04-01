using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum MLKEMDecapsulationDisposition
{
    [EnumMember(Value = "no modification")]
    None,
    
    [EnumMember(Value = "modify ciphertext")]
    ModifyCiphertext
}
