using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public record HssSignatureResult : IHssSignatureResult
    {
        /// <summary>
        /// This constructor indicates the <see cref="IHssPrivateKey"/> was exhausted,
        /// and can no longer be used for signatures.
        /// </summary>
        public HssSignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsExhausted = true;
        }

        /// <summary>
        /// Provides a signature to this instance.
        /// This constructor indicates the <see cref="IHssPrivateKey"/> is not exhausted,
        /// and was able to provide a valid key for signing.
        /// </summary>
        /// <param name="signature">The signature to return.</param>
        public HssSignatureResult(byte[] signature)
        {
            Signature = signature;
        }

        public string ErrorMessage { get; }
        public bool IsExhausted { get; }
        public byte[] Signature { get; }
    }
}
