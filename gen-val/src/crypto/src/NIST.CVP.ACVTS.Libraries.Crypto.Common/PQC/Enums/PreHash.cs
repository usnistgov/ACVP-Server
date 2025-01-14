using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;

public enum PreHash
{
    [EnumMember(Value = "none")]
    None,
    
    [EnumMember(Value = "pure")]
    Pure,
    
    [EnumMember(Value = "preHash")]
    PreHash
}
