using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native
{
    /// <summary>
    /// A <see cref="IXmssSigner" /> result.
    /// </summary>
    public interface IXmssSignatureResult
    {
        /// <summary>
        /// True when the <see cref="IXmssPrivateKey"/> has been completely exhausted of
        /// unused leaf indices, the cap of which is 2^h.
        /// </summary>
        bool Exhausted { get; }
        /// <summary>
        /// The signature of the signed message,
        /// can be null if the <see cref="IXmssPrivateKey"/> is exhausted of leaf indices.
        /// </summary>
        byte[] Signature { get; }
    }
}
