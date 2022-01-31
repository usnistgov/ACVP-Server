using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum MacSaltMethod
    {
        /// <summary>
        /// No salt is used.
        /// </summary>
        [EnumMember(Value = "none")]
        None,
        /// <summary>
        /// The default salt of all 0 bits is used.
        /// </summary>
        [EnumMember(Value = "default")]
        Default,
        /// <summary>
        /// The salt is randomly generated and included in the otherInput
        /// </summary>
        [EnumMember(Value = "random")]
        Random
    }
}
