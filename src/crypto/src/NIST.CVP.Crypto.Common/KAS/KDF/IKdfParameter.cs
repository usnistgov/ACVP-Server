using NIST.CVP.Crypto.Common.KAS.Enums;

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
        /// Accepts a <see cref="IKdfVisitor"/> this will in turn dispatch a call to a supported KDF.
        /// </summary>
        /// <param name="visitor">Describes how to invoke a KDF for implementors.</param>
        /// <returns>A derived key.</returns>
        KdfResult AcceptKdf(IKdfVisitor visitor);
    }
}