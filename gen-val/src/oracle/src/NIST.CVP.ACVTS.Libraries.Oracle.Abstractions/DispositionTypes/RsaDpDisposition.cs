using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum RsaDpDisposition
    {
        [EnumMember(Value = "none")]
        None,
        
        [EnumMember(Value = "Failure - CipherText is equal to 0")]
        CtEqual0,
        
        [EnumMember(Value = "Failure - CipherText is equal to 1")]
        CtEqual1,
        
        [EnumMember(Value = "Failure - CipherText is equal to N - 1")]
        CtEqualNMinusOne,
        
        [EnumMember(Value = "Failure - CipherText is > N -1")]
        CtGreaterNMinusOne,
        
    }
}
