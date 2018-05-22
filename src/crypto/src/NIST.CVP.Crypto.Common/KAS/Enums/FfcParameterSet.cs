using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The allowed Parameter Sets for in use in FFC KAS
    /// </summary>
    public enum FfcParameterSet
    {
        [EnumMember(Value = "fb")]
        Fb,
        [EnumMember(Value = "fc")]
        Fc
    }
}