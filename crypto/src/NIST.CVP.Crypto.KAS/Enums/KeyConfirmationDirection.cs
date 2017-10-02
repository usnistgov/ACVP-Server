using System.ComponentModel;

namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The type of Key Confirmation that is to occur
    /// </summary>
    public enum KeyConfirmationDirection
    {
        /// <summary>
        /// No key confirmation is performed
        /// </summary>
        None,
        /// <summary>
        /// Key Confirmation occurs only in one direction
        /// </summary>
        [Description("unilateral")]
        Unilateral,
        /// <summary>
        /// Key Confirmation occurs in both directions.
        /// </summary>
        [Description("bilateral")]
        Bilateral
    }
}