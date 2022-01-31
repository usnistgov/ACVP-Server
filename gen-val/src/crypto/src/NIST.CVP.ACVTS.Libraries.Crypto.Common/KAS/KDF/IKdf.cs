using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF
{
    /// <summary>
    /// Interface for deriving a key from varying parameters.
    /// </summary>
    public interface IKdf
    {
        /// <summary>
        /// The enum type of KDF.
        /// </summary>
        Kda KdfType { get; }
        /// <summary>
        /// Using the provided parameters, derive a key. 
        /// </summary>
        /// <param name="param">The parameters to use for deriving a key.</param>
        /// <param name="fixedInfo">The contextual fixed info that goes into the kdf.</param>
        /// <returns>The derived key.</returns>
        KdfResult DeriveKey(IKdfParameter param, BitString fixedInfo);
    }
}
