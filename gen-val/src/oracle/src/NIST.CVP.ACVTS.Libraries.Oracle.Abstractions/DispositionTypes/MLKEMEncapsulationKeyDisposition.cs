using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

public enum MLKEMEncapsulationKeyDisposition
{
    [EnumMember(Value = "valid encapsulation key")]
    None,
    
    [EnumMember(Value = "noisy linear system values too large")]
    ValuesTooLarge,

    [EnumMember(Value = "invalid encapsulation key - too short")]
    TooShort,

    [EnumMember(Value = "invalid encapsulation key - too long")]
    TooLong,
}
