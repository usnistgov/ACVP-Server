using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Abstraction for the HSS signature verification process.
    /// </summary>
    public interface IHssVerifier
    {
        /// <summary>
        /// Verify a <see cref="message"/>'s <see cref="signature"/>,
        /// ensuring it was signed by the <see cref="IHssPrivateKey"/> corresponding to the provided <see cref="IHssPublicKey"/>.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-6.3
        /// </summary>
        /// <param name="publicKey">The <see cref="IHssPublicKey"/> to use for verification.</param>
        /// <param name="signature">The signature produced purportedly by signing <see cref="message"/> with the corresponding <see cref="publicKey"/>'s <see cref="IHssPrivateKey"/>.</param>
        /// <param name="message">The message in which to verify the <see cref="signature"/> on.</param>
        /// <returns>True if the signature is valid, otherwise false</returns>
        Task<bool> Verify(IHssPublicKey publicKey, byte[] signature, byte[] message);
    }
}
