using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The allowed Parameter Sets for in use in FFC KAS
    /// </summary>
    public enum FfcParameterSet
    {
        [Description("fb")]
        Fb,
        [Description("fc")]
        Fc
    }
}