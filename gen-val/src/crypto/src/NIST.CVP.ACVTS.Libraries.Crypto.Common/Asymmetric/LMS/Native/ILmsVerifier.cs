using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Describes the LMS signature verification process
    /// </summary>
    public interface ILmsVerifier
    {
        /// <summary>
        /// Verify a <see cref="message"/>'s <see cref="signature"/>,
        /// ensuring it was signed by the <see cref="ILmsPrivateKey"/> corresponding to the provided <see cref="ILmsPublicKey"/>.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-5.4.2
        /// </summary>
        /// <param name="lmsPublicKey">The public key of the LMS tree</param>
        /// <param name="signature">The signature to verify</param>
        /// <param name="message">The message that was purportedly signed by the private key paired to <see cref="lmsPublicKey"/>.</param>
        /// <returns>True if the signature is valid, otherwise false</returns>
        LmsVerificationResult Verify(ILmsPublicKey lmsPublicKey, byte[] signature, byte[] message);
    }
}
