using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Abstraction fo the LM-OTS signature signing process.
    /// </summary>
    public interface ILmOtsSigner
    {
        /// <summary>
        /// Sign a <see cref="message"/> using a <see cref="ILmOtsPrivateKey"/> and randomizer <see cref="c"/>.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-4.5
        /// </summary>
        /// <param name="privateKey">The <see cref="ILmOtsPrivateKey"/> to sign the <see cref="message"/> with.</param>
        /// <param name="randomizerC">The randomizer implementation to use in signing the <see cref="message"/>.</param>
        /// <param name="message">The message in which to produce a signature for.</param>
        /// <returns>The signature of the signed <see cref="message"/>.</returns>
        byte[] Sign(ILmOtsPrivateKey privateKey, ILmOtsRandomizerC randomizerC, byte[] message);
    }
}
