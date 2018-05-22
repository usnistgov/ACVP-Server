using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The type of Key Confirmation that is to occur
    /// </summary>
    public enum KeyConfirmationDirection
    {
        /// <summary>
        /// No key confirmation is performed
        /// </summary>
        [EnumMember(Value = "")]
        None,
        /// <summary>
        /// Key Confirmation occurs only in one direction
        /// </summary>
        [EnumMember(Value = "unilateral")]
        Unilateral,
        /// <summary>
        /// Key Confirmation occurs in both directions.
        /// </summary>
        [EnumMember(Value = "bilateral")]
        Bilateral
    }
}