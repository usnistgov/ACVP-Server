using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native
{
    public record XmssSignatureResult : IXmssSignatureResult
    {
        /// <summary>
        /// This constructor indicates the <see cref="IXmssPrivateKey"/> was exhausted,
        /// and can no longer be used for signatures.
        /// </summary>
        public XmssSignatureResult()
        {
            Exhausted = true;
        }

        /// <summary>
        /// Provides a signature to this instance.
        /// This constructor indicates the <see cref="IXmssPrivateKey"/> is not exhausted,
        /// and was able to provide a valid leaf index for signing.
        /// </summary>
        /// <param name="signature">The signature to return.</param>
        public XmssSignatureResult(byte[] signature)
        {
            Signature = signature;
        }

        public bool Exhausted { get; }
        public byte[] Signature { get; }
    }
}
