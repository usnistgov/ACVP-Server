using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums
{
    public enum GeneratorGenMode
    {
        [Description("none")]
        None,

        [Description("unverifiable")]
        Unverifiable,

        [Description("canonical")]
        Canonical
    }

    public enum PrimeGenMode
    {
        [Description("none")]
        None,

        [Description("probable")]
        Probable,

        [Description("provable")]
        Provable
    }
}
