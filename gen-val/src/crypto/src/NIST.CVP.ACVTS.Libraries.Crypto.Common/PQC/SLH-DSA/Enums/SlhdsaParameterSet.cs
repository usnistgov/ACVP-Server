using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;

/// <summary>
/// Parameter set labels from FIPS 205 Section 10.
/// </summary>
public enum SlhdsaParameterSet
{
    None,
    
    [EnumMember(Value = "SLH-DSA-SHA2-128s")]
    SLH_DSA_SHA2_128s,
    [EnumMember(Value = "SLH-DSA-SHAKE-128s")]
    SLH_DSA_SHAKE_128s,
    
    [EnumMember(Value = "SLH-DSA-SHA2-128f")]
    SLH_DSA_SHA2_128f,
    [EnumMember(Value = "SLH-DSA-SHAKE-128f")]
    SLH_DSA_SHAKE_128f,
    
    [EnumMember(Value = "SLH-DSA-SHA2-192s")]
    SLH_DSA_SHA2_192s,
    [EnumMember(Value = "SLH-DSA-SHAKE-192s")]
    SLH_DSA_SHAKE_192s,
    
    [EnumMember(Value = "SLH-DSA-SHA2-192f")]
    SLH_DSA_SHA2_192f,
    [EnumMember(Value = "SLH-DSA-SHAKE-192f")]
    SLH_DSA_SHAKE_192f,
    
    [EnumMember(Value = "SLH-DSA-SHA2-256s")]
    SLH_DSA_SHA2_256s,
    [EnumMember(Value = "SLH-DSA-SHAKE-256s")]
    SLH_DSA_SHAKE_256s,
    
    [EnumMember(Value = "SLH-DSA-SHA2-256f")]
    SLH_DSA_SHA2_256f,
    [EnumMember(Value = "SLH-DSA-SHAKE-256f")]
    SLH_DSA_SHAKE_256f
}

