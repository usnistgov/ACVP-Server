using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums
{
    public enum GeneratorGenMode
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "unverifiable")]
        Unverifiable,

        [EnumMember(Value = "canonical")]
        Canonical
    }

    public enum PrimeGenMode
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "probable")]
        Probable,

        [EnumMember(Value = "provable")]
        Provable
    }
}
