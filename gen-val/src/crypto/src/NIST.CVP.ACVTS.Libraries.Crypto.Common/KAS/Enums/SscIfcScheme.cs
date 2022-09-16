using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum SscIfcScheme
    {
        [EnumMember(Value = "none")]
        None,
        [EnumMember(Value = "KAS1")]
        Kas1,
        [EnumMember(Value = "KAS2")]
        Kas2
    }
}
