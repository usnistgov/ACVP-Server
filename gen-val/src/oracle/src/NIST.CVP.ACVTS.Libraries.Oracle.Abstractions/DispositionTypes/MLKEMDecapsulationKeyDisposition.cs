using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum MLKEMDecapsulationKeyDisposition
{
    [EnumMember(Value = "valid decapsulation key")]
    None,
    
    [EnumMember(Value = "modified H")]
    ModifyH
}
