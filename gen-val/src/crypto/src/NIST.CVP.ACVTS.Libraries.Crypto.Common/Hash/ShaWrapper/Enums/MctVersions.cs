using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums
{
    public enum MctVersions
    {
        [EnumMember(Value = "standard")]
        Standard,
        [EnumMember(Value = "alternate")]
        Alternate
    }
}
