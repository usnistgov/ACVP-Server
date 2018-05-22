using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The parties involved in Key Confirmation
    /// </summary>
    public enum KeyConfirmationRole
    {
        /// <summary>
        /// No KeyConfirmation is performed
        /// </summary>
        [EnumMember(Value = "")]
        None,
        /// <summary>
        /// The provider of the Key that is to be confirmed
        /// </summary>
        [EnumMember(Value = "provider")]
        Provider,
        /// <summary>
        /// The recipient of the Key that is to be confirmed
        /// </summary>
        [EnumMember(Value = "recipient")]
        Recipient
    }
}