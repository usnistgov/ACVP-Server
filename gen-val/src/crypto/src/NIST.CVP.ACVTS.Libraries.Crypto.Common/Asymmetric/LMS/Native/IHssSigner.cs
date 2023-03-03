using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Abstraction for the HSS signing process.
    /// </summary>
    public interface IHssSigner
    {
        /// <summary>
        /// Signs a <see cref="message"/> using <see cref="keyPair"/> and <see cref="c"/>.
        /// 
        /// https://datatracker.ietf.org/doc/html/rfc8554#section-6.2
        /// </summary>
        /// <param name="keyPair">The <see cref="IHssKeyPair"/> used to sign the message.</param>
        /// <param name="randomizerC">The randomizer implementation to use in signing the <see cref="message"/>.</param>
        /// <param name="message">The message to sign.</param>
        /// <param name="preSignIncrementQ">
        ///		Increment the Q value on the active LMS tree, and subsequent higher level trees,
        ///		prior to performing the sign operation.  Any value aside from 0 is a
        ///		means of testing new LMS tree generation in cases of tree exhaustion.
        /// </param>
        /// <returns>The signature of the signed message.</returns>
        Task<IHssSignatureResult> Sign(IHssKeyPair keyPair, ILmOtsRandomizerC randomizerC, byte[] message, int preSignIncrementQ = 0);
    }
}
