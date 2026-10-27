namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native
{
    /// <summary>
    /// Provides a means of verifying an XMSS signature.
    /// </summary>
    public interface IXmssVerifier
    {
        /// <summary>
        /// Verify a signature against a public key and message.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-4.1.10 Algorithm 14.
        /// </summary>
        /// <param name="publicKey">The full wire representation of the public key, OID || root || SEED.</param>
        /// <param name="signature">The signature, idx_sig || r || sig_ots || auth.</param>
        /// <param name="message">The purportedly signed message.</param>
        /// <returns>A <see cref="XmssVerificationResult"/> describing success or failure.</returns>
        XmssVerificationResult Verify(byte[] publicKey, byte[] signature, byte[] message);
    }
}
