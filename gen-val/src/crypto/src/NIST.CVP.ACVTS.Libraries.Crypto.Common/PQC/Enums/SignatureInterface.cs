using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;

public enum SignatureInterface
{
    None,
    
    [EnumMember(Value = "internal")]
    Internal,
    
    [EnumMember(Value = "external")]
    External
}
