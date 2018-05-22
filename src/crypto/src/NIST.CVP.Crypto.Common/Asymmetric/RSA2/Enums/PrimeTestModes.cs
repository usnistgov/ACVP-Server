using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum PrimeTestModes
    {
        [EnumMember(Value = "tblC2")]
        C2,

        [EnumMember(Value = "tblC3")]
        C3
    }
}
