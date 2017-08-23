namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The type of Key Confirmation that is to occur
    /// </summary>
    public enum KeyConfirmationDirection
    {
        /// <summary>
        /// Key Confirmation occurs only in one direction
        /// </summary>
        Unilateral,
        /// <summary>
        /// Key Confirmation occurs in both directions.
        /// </summary>
        Bilateral
    }
}