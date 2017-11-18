using System.ComponentModel;

namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The allowed Parameter Sets for in use in ECC KAS
    /// </summary>
    public enum EccParameterSet
    {
        [Description("eb")]
        Eb,
        [Description("ec")]
        Ec,
        [Description("ed")]
        Ed,
        [Description("ee")]
        Ee
    }
}