using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers
{
    /// <summary>
    /// Represents the attributes tied to a LM-OTS parameter set.
    /// </summary>
    /// <param name="Mode"><inheritdoc cref="Mode"/></param>
    /// <param name="NumericIdentifier"><inheritdoc cref="NumericIdentifier"/></param>
    /// <param name="N"><inheritdoc cref="N"/></param>
    /// <param name="W"><inheritdoc cref="W"/></param>
    /// <param name="P"><inheritdoc cref="P"/></param>
    /// <param name="U"><inheritdoc cref="U"/></param>
    /// <param name="V"><inheritdoc cref="V"/></param>
    /// <param name="LeftShift"><inheritdoc cref="LeftShift"/></param>
    /// <param name="ShaMode"><inheritdoc cref="ShaMode"/></param>
    public record LmOtsAttribute(LmOtsMode Mode, byte[] NumericIdentifier, int N, int W, int P, int U, int V, int LeftShift, ModeValues ShaMode)
    {
        /// <summary>
        /// The parameter set attributes being described
        /// </summary>
        public LmOtsMode Mode { get; } = Mode;
        /// <summary>
        /// The byte representation of the parameter set as described in:
        /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf
        /// https://datatracker.ietf.org/doc/html/rfc8554
        /// https://www.iana.org/assignments/leighton-micali-signatures/leighton-micali-signatures.xhtml
        /// </summary>
        public byte[] NumericIdentifier { get; } = NumericIdentifier;

        /// <summary>
        /// The number of bytes in the hash function used with this parameter set
        /// </summary>
        public int N { get; } = N;

        /// <summary>
        /// In LMS, the number of bits from the hash or checksum used in a single Winternitz chain.
        /// The length of a Winternitz chain is 2w. (Note that using a Winternitz parameter of w = 4 in LMS would be
        /// comparable to using a parameter of w = 16 in XMSS.)
        /// </summary>
        public int W { get; } = W;

        /// <summary>
        /// The number of n-byte string elements in an LM-OTS private key, public key, and signature.
        /// </summary>
        public int P { get; } = P;

        /// <summary>
        /// The number of w-bit fields required to contain the hash of the message
        /// </summary>
        public int U { get; } = U;

        /// <summary>
        /// The number of w-bit fields required to contain the checksum byte strings.
        /// </summary>
        public int V { get; } = V;

        /// <summary>
        /// This is the size of the shift needed to move the checksum so
        /// that it appears in the checksum digits.
        /// </summary>
        public int LeftShift { get; } = LeftShift;

        /// <summary>
        /// The underlying SHA algorithm used for the <see cref="LmOtsMode"/>
        /// </summary>
        public ModeValues ShaMode { get; } = ShaMode;
    }
}
