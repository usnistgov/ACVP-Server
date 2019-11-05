using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeTestFips186_4Modes
    {
        Invalid,
        
        [EnumMember(Value = "tblC2")]
        TblC2,
        
        [EnumMember(Value = "tblC3")]
        TblC3
    }
}