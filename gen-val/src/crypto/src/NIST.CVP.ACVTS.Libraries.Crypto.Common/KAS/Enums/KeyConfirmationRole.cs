using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// The parties involved in Key Confirmation
    /// </summary>
    public enum KeyConfirmationRole
    {
        /// <summary>
        /// No KeyConfirmation is performed
        /// </summary>
        [EnumMember(Value = "none")]
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
