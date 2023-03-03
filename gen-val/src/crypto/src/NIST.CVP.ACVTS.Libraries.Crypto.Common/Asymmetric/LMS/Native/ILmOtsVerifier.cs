using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Abstraction for the LM-OTS signature verification process.
    /// </summary>
    public interface ILmOtsVerifier
    {
        /// <summary>
        /// Verify a <see cref="message"/>'s <see cref="signature"/>,
        /// ensuring it was signed by the <see cref="ILmOtsPrivateKey"/> corresponding to the provided <see cref="ILmOtsPublicKey"/>.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-4.6
        /// </summary>
        /// <param name="publicKey">The <see cref="ILmOtsPublicKey"/> to use for verification.</param>
        /// <param name="signature">The signature produced purportedly by signing <see cref="message"/> with the corresponding <see cref="publicKey"/>'s <see cref="ILmOtsPrivateKey"/>.</param>
        /// <param name="message">The message in which to verify the <see cref="signature"/> on.</param>
        /// <returns>True if the signature is valid, otherwise false</returns>
        bool Verify(ILmOtsPublicKey publicKey, byte[] signature, byte[] message);
    }
}
