using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native
{
    /// <summary>
    /// The WOTS+ one-time signature operations underlying XMSS,
    /// as described in https://datatracker.ietf.org/doc/html/rfc8391#section-3.
    ///
    /// All operations are keyed by a secret seed and public seed, with hash inputs
    /// domain separated by a hash address (ADRS) identifying the chain and step in use.
    /// </summary>
    public interface IWotsPlus
    {
        /// <summary>
        /// Generate a WOTS+ public key: len chains of n bytes each, flattened.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-3.1.4 Algorithm 4.
        /// </summary>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="skSeed">The n-byte secret seed the chain secrets are derived from.</param>
        /// <param name="pubSeed">The n-byte public seed used for hash keys and bitmasks.</param>
        /// <param name="adrs">The eight word OTS hash address identifying this key pair; mutated during computation.</param>
        /// <returns>The len * n byte public key.</returns>
        byte[] PkGen(XmssAttribute attribute, byte[] skSeed, byte[] pubSeed, uint[] adrs);

        /// <summary>
        /// Sign an n-byte message digest, producing len chains advanced to the digest's base-w digits.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-3.1.5 Algorithm 5.
        /// </summary>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="message">The n-byte message digest to sign.</param>
        /// <param name="skSeed">The n-byte secret seed the chain secrets are derived from.</param>
        /// <param name="pubSeed">The n-byte public seed used for hash keys and bitmasks.</param>
        /// <param name="adrs">The eight word OTS hash address identifying this key pair; mutated during computation.</param>
        /// <returns>The len * n byte signature.</returns>
        byte[] Sign(XmssAttribute attribute, byte[] message, byte[] skSeed, byte[] pubSeed, uint[] adrs);

        /// <summary>
        /// Compute the WOTS+ public key candidate from a signature and message digest by completing the chains.
        ///
        /// https://datatracker.ietf.org/doc/html/rfc8391#section-3.1.6 Algorithm 6.
        /// </summary>
        /// <param name="attribute">The XMSS parameter set attributes in use.</param>
        /// <param name="signature">The len * n byte signature.</param>
        /// <param name="message">The n-byte message digest purportedly signed.</param>
        /// <param name="pubSeed">The n-byte public seed used for hash keys and bitmasks.</param>
        /// <param name="adrs">The eight word OTS hash address identifying this key pair; mutated during computation.</param>
        /// <returns>The len * n byte public key candidate.</returns>
        byte[] PkFromSig(XmssAttribute attribute, byte[] signature, byte[] message, byte[] pubSeed, uint[] adrs);
    }
}
