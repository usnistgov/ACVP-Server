using System.Runtime.Serialization;

namespace NIST.CVP.Pools.Enums
{
    public enum PoolTypes
    {
        [EnumMember(Value = "sha")]
        SHA = 1,

        [EnumMember(Value = "aes")]
        AES = 2
    }
}
