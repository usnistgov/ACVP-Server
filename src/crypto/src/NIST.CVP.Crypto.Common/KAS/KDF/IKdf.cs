using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Interface for deriving a key from varying parameters.
    /// </summary>
    public interface IKdf
    {
        /// <summary>
        /// The enum type of KDF.
        /// </summary>
        KasKdf KdfType { get; }
        /// <summary>
        /// Using the provided parameters, derive a key. 
        /// </summary>
        /// <param name="param">The parameters to use for deriving a key.</param>
        /// <returns>The derived key.</returns>
        KdfResult DeriveKey(IKdfParameter param);
    }
}