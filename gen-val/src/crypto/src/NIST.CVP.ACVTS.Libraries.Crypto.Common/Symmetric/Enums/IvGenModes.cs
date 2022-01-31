using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums
{
    public enum IvGenModes
    {
        None,
        /// <summary>
        /// The IV can be generated internally to the IUT for encrypt operations.
        /// </summary>
        [EnumMember(Value = "internal")]
        Internal,
        /// <summary>
        /// The IV is generated external to the IUT for encrypt operations.
        /// </summary>
        [EnumMember(Value = "external")]
        External,
    }
}
