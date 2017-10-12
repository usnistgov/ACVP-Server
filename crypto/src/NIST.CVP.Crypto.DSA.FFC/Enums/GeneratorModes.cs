using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.Enums
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
