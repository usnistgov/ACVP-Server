using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Represents information needed for invoking a KDF function
    /// </summary>
    public interface IKdfParameter
    {
        /// <summary>
        /// The type of KDF supported.
        /// </summary>
        KasKdf KdfType { get; }
        /// <summary>
        /// The shared secret for use in deriving a key.
        /// </summary>
        BitString Z { get; set; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        int L { get; set; }
        /// <summary>
        /// The pattern to use when constructing fixed info.
        /// </summary>
        string FixedInfoPattern { get; set; }
        /// <summary>
        /// Accepts a <see cref="IKdfVisitor"/> this will in turn dispatch a call to a supported KDF.
        /// </summary>
        /// <param name="visitor">Describes how to invoke a KDF for implementors.</param>
        /// <param name="fixedInfo">The contextual fixed information to be plugged into a kdf.</param>
        /// <returns>A derived key.</returns>
        KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo);
    }
}