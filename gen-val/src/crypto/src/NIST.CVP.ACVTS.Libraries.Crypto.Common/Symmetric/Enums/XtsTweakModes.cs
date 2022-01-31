using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums
{
    public enum XtsTweakModes
    {
        [EnumMember(Value = "hex")]
        Hex,

        [EnumMember(Value = "number")]
        Number
    }
}
