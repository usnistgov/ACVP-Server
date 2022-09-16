using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums
{
    public enum KasAlgorithm
    {
        [EnumMember(Value = "none")]
        None,
        
        [EnumMember(Value = "FFC")]
        Ffc,
        
        [EnumMember(Value = "ECC")]
        Ecc,
    }
}
