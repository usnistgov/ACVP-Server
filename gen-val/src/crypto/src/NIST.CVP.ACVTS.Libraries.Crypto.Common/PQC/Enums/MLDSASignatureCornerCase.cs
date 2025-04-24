using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;

public enum MLDSASignatureCornerCase
{
    [EnumMember(Value = "none")]
    None,
    
    [EnumMember(Value = "all-rejection")]
    AllRejectionCheck,
    
    [EnumMember(Value = "total-rejection-count")]
    TotalRejection
}
