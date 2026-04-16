using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;

public enum MLKEMParameterSet
{
    None,
    
    [EnumMember(Value = "ML-KEM-512")]
    ML_KEM_512,
    
    [EnumMember(Value = "ML-KEM-768")]
    ML_KEM_768,

    [EnumMember(Value = "ML-KEM-1024")]
    ML_KEM_1024
}
