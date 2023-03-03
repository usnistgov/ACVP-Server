using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// A <see cref="ILmsSigner" /> result.
    /// </summary>
    public interface ILmsSignatureResult
    {
        /// <summary>
        /// True when the <see cref="ILmsPrivateKey"/> has been completely exhausted of
        /// unused "Q" values, the cap of which is 2^h.
        /// </summary>
        bool Exhausted { get; }
        /// <summary>
        /// The signature of the signed message,
        /// can be null if the <see cref="ILmsPrivateKey"/> is exhausted of "Q" values.
        /// </summary>
        byte[] Signature { get; }
    }
}
