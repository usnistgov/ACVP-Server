using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums
{
    public enum PrimeTestFips186_4Modes
    {
        [EnumMember(Value = "invalid")]
        Invalid,

        [EnumMember(Value = "tblC2")]
        TblC2,

        [EnumMember(Value = "tblC3")]
        TblC3
    }
}
