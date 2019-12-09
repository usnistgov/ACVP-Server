using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The allowed Parameter Sets for in use in ECC KAS
    /// </summary>
    public enum EccParameterSet
    {
        [EnumMember(Value = "eb")]
        Eb,
        [EnumMember(Value = "ec")]
        Ec,
        [EnumMember(Value = "ed")]
        Ed,
        [EnumMember(Value = "ee")]
        Ee
    }
}