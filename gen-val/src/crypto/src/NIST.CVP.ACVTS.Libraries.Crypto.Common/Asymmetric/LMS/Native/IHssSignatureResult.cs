using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native
{
    /// <summary>
    /// A <see cref="IHssSigner" /> result.
    /// </summary>
    public interface IHssSignatureResult
    {
        /// <summary>
        /// True when the <see cref="IHssKeyPair"/> has been completely exhausted of
        /// <see cref="ILmsKeyPair"/>s and <see cref="ILmOtsKeyPair"/>s.
        /// </summary>
        bool IsExhausted { get; }
        /// <summary>
        /// The signature of the signed message, can be null if the <see cref="IHssKeyPair"/> is exhausted.
        /// </summary>
        byte[] Signature { get; }
    }
}
