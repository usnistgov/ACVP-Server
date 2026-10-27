using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers
{
    /// <summary>
    /// Represents the attributes tied to an XMSS parameter set.
    /// </summary>
    /// <param name="Mode"><inheritdoc cref="Mode"/></param>
    /// <param name="NumericIdentifier"><inheritdoc cref="NumericIdentifier"/></param>
    /// <param name="N"><inheritdoc cref="N"/></param>
    /// <param name="H"><inheritdoc cref="H"/></param>
    /// <param name="W"><inheritdoc cref="W"/></param>
    /// <param name="Len1"><inheritdoc cref="Len1"/></param>
    /// <param name="Len2"><inheritdoc cref="Len2"/></param>
    /// <param name="PaddingLength"><inheritdoc cref="PaddingLength"/></param>
    /// <param name="ShaMode"><inheritdoc cref="ShaMode"/></param>
    public record XmssAttribute(XmssMode Mode, byte[] NumericIdentifier, int N, int H, int W, int Len1, int Len2,
        int PaddingLength, ModeValues ShaMode)
    {
        /// <summary>
        /// The parameter set attributes being described
        /// </summary>
        public XmssMode Mode { get; } = Mode;

        /// <summary>
        /// The byte representation of the parameter set as described in:
        /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf
        /// https://datatracker.ietf.org/doc/html/rfc8391
        /// https://www.iana.org/assignments/xmss-extended-hash-based-signatures/xmss-extended-hash-based-signatures.xhtml
        /// </summary>
        public byte[] NumericIdentifier { get; } = NumericIdentifier;

        /// <summary>
        /// The number of bytes associated with each node of the tree and each hash output.
        /// </summary>
        public int N { get; } = N;

        /// <summary>
        /// The height of the Merkle tree.
        /// </summary>
        public int H { get; } = H;

        /// <summary>
        /// The Winternitz parameter; the number of values a WOTS+ chain digit can take.
        /// </summary>
        public int W { get; } = W;

        /// <summary>
        /// The number of w-base digits taken from the message digest for WOTS+ chains.
        /// </summary>
        public int Len1 { get; } = Len1;

        /// <summary>
        /// The number of w-base digits used for the WOTS+ checksum.
        /// </summary>
        public int Len2 { get; } = Len2;

        /// <summary>
        /// The total number of WOTS+ chains, len1 + len2.
        /// </summary>
        public int Len => Len1 + Len2;

        /// <summary>
        /// The byte length of the domain separation prefix toByte(X, PaddingLength) used by the
        /// PRF/F/H/H_msg constructions; 32 bytes for the 256-bit parameter sets, 4 bytes for the
        /// 192-bit parameter sets per SP 800-208 section 5.3.
        /// </summary>
        public int PaddingLength { get; } = PaddingLength;

        /// <summary>
        /// The underlying SHA algorithm used for the <see cref="XmssMode"/>
        /// </summary>
        public ModeValues ShaMode { get; } = ShaMode;
    }
}
