using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// Exposes a means of getting a randomizer "C" value for use in message signing with LM-OTS keys.
    /// </summary>
    public interface ILmOtsRandomizerC
    {
        /// <summary>
        /// Get a "C" value for signing messages.
        /// </summary>
        /// <param name="privateKey">The <see cref="ILmOtsPrivateKey"/> to get a "C" value for.</param>
        /// <returns>The randomizer value "C".</returns>
        byte[] GetRandomizerValueC(ILmOtsPrivateKey privateKey);
    }
}
