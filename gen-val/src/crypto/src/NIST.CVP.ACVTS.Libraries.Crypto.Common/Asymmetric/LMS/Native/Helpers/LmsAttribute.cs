using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers
{
    /// <summary>
    /// Represents the attributes tied to a LMS parameter set.
    /// </summary>
    /// <param name="Mode"><inheritdoc cref="Mode"/></param>
    /// <param name="NumericIdentifier"><inheritdoc cref="NumericIdentifier"/></param>
    /// <param name="M"><inheritdoc cref="M"/></param>
    /// <param name="H"><inheritdoc cref="H"/></param>
    /// <param name="ShaMode"><inheritdoc cref="ShaMode"/></param>
    public record LmsAttribute(LmsMode Mode, byte[] NumericIdentifier, int M, int H, ModeValues ShaMode)
    {
        /// <summary>
        /// The parameter set attributes being described
        /// </summary>
        public LmsMode Mode { get; } = Mode;

        /// <summary>
        /// The byte representation of the parameter set as described in:
        /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf
        /// https://datatracker.ietf.org/doc/html/rfc8554
        /// https://www.iana.org/assignments/leighton-micali-signatures/leighton-micali-signatures.xhtml
        /// </summary>
        public byte[] NumericIdentifier { get; } = NumericIdentifier;

        /// <summary>
        /// The number of bytes associated with each node of a Merkle tree.
        /// </summary>
        public int M { get; } = M;

        /// <summary>
        /// The height of the Merkle tree.
        /// </summary>
        public int H { get; } = H;

        /// <summary>
        /// The underlying SHA algorithm used for the <see cref="LmsMode"/>
        /// </summary>
        public ModeValues ShaMode { get; } = ShaMode;
    }
}
