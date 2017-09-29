namespace NIST.CVP.Crypto.KAS.Enums
{
    /// <summary>
    /// The "modes" that an <see cref="IKas"/> can run in.
    /// </summary>
    public enum KasMode
    {
        /// <summary>
        /// No Kdf, No Key confirmation (component only test)
        /// </summary>
        NoKdfNoKc,
        /// <summary>
        /// No Key Confirmation test - uses a MAC function and KDF on the generated secret
        /// Kdf, No Key Confirmation
        /// </summary>
        KdfNoKc,
        /// <summary>
        /// Key Confirmation test - uses a MAC function, KDF, and Key Confirmation
        /// Kdf, Key Confirmation
        /// </summary>
        KdfKc
    }
}