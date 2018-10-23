using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Hash.SHA2
{
    public enum ModeValues
    {
        [EnumMember(Value = "SHA-1")]
        SHA1,
        [EnumMember(Value = "SHA")]
        SHA2,
        NONE
    }
}