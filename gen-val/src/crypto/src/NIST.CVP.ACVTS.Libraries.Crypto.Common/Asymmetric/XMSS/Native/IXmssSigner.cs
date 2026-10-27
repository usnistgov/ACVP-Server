using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native
{
    /// <summary>
    /// Provides a means of signing a message with an XMSS private key.
    /// </summary>
    public interface IXmssSigner
    {
        /// <summary>
        /// Sign a message with the provided <see cref="IXmssPrivateKey"/>,
        /// consuming the private key's next available leaf index.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-4.1.9 Algorithm 12.
        /// </summary>
        /// <param name="privateKey">The private key to sign with.</param>
        /// <param name="message">The message to sign.</param>
        /// <returns>The signature result, marked exhausted when no leaf indices remain.</returns>
        IXmssSignatureResult Sign(IXmssPrivateKey privateKey, byte[] message);
    }
}
