using System.ComponentModel;

namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The parties involved in Key Confirmation
    /// </summary>
    public enum KeyConfirmationRole
    {
        /// <summary>
        /// No KeyConfirmation is performed
        /// </summary>
        None,
        /// <summary>
        /// The provider of the Key that is to be confirmed
        /// </summary>
        [Description("provider")]
        Provider,
        /// <summary>
        /// The recipient of the Key that is to be confirmed
        /// </summary>
        [Description("recipient")]
        Recipient
    }
}