using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public record LmsSignatureResult : ILmsSignatureResult
    {
        /// <summary>
        /// This constructor indicates the <see cref="ILmsPrivateKey"/> was exhausted,
        /// and can no longer be used for signatures.
        /// </summary>
        public LmsSignatureResult()
        {
            Exhausted = true;
        }

        /// <summary>
        /// Provides a signature to this instance.
        /// This constructor indicates the <see cref="ILmsPrivateKey"/> is not exhausted,
        /// and was able to provide a valid "Q" for signing.
        /// </summary>
        /// <param name="signature">The signature to return.</param>
        public LmsSignatureResult(byte[] signature)
        {
            Signature = signature;
        }

        public bool Exhausted { get; }
        public byte[] Signature { get; }
    }
}
