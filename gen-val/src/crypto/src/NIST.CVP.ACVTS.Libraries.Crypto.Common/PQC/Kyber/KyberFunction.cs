using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;

public enum KyberFunction
{
    None,
    
    [EnumMember(Value = "encapsulation")]
    Encapsulation,
    
    [EnumMember(Value = "decapsulation")]
    Decapsulation,
    
    [EnumMember(Value = "encapsulationKeyCheck")]
    EncapsulationKeyCheck,
    
    [EnumMember(Value = "decapsulationKeyCheck")]
    DecapsulationKeyCheck,
}
