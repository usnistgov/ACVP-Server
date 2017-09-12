namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The "modes" that an <see cref="IKas"/> can run in, depending on the <see cref="IKasParameters"/> used to retrieve an instance.
    /// </summary>
    public enum KasMode
    {
        /// <summary>
        /// Component only test - uses a hash function on the generated secret.
        /// No KDF, No Key Confirmation
        /// </summary>
        ComponentOnly,
        /// <summary>
        /// No Key Confirmation test - uses a MAC function and KDF on the generated secret
        /// Kdf, No Key Confirmation
        /// </summary>
        NoKeyConfirmation,
        /// <summary>
        /// Key Confirmation test - uses a MAC function, KDF, and Key Confirmation
        /// Kdf, Key Confirmation
        /// </summary>
        KeyConfirmation
    }
}