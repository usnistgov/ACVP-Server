using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Abstraction for the LMS signing process.
    /// </summary>
    public interface ILmsSigner
    {
        /// <summary>
        /// Sign a <see cref="message"/> using <see cref="ILmsPrivateKey"/> and randomizer <see cref="c"/>.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-5.4.1
        /// </summary>
        /// <param name="privateKey">The <see cref="ILmsPrivateKey"/> to sign the <see cref="message"/> with.</param>
        /// <param name="randomizerC">The randomizer implementation to use in signing the <see cref="message"/>.</param>
        /// <param name="message">The message in which to produce a signature for.</param>
        /// <returns>
        ///		a <see cref="ILmsSignatureResult"/>.
        ///		When a Signature is present, the <see cref="privateKey"/> was able to produce a signature,
        ///		otherwise it was exhausted.
        /// </returns>
        ILmsSignatureResult Sign(ILmsPrivateKey privateKey, ILmOtsRandomizerC randomizerC, byte[] message);
    }
}
